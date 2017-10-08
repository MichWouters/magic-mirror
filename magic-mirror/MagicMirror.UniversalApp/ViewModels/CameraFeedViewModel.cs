using MagicMirror.Business.Cognitive;
using MagicMirror.Business.Models.User;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class CameraFeedViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        protected CancellationTokenSource _requestStopCancellationToken;
        protected CaptureElement _captureElement;
        protected MediaCapture _mediaCapture;
        private readonly FaceService _faceService;
        private readonly UserService _userService;
        private UserViewModel _user = new UserViewModel();
        private string _apiOfflineText;

        public UserViewModel User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public CaptureElement CaptureElement
        {
            get
            {
                return _captureElement;
            }
        }

        public string ApiOfflineText
        {
            get
            {
                return _apiOfflineText;
            }
            set
            {
                _apiOfflineText = value;
                OnPropertyChanged();
            }
        }

        public CameraFeedViewModel()
        {
            User.FirstName = "John";
            User.LastName = "Doe";
            var sqlContext = new SqliteContext();
            _userService = new UserService(sqlContext);
            _faceService = new FaceService();

            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    await InitializeCamera();
                    ApiOfflineText = "";
                }
                catch (Exception ex)
                {
                    if (ex is System.Net.Http.HttpRequestException)
                    {
                        ApiOfflineText = "Offline";
                    }
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        private async Task InitializeCamera()
        {
            _requestStopCancellationToken = new CancellationTokenSource();
            _captureElement = new CaptureElement();
            var videoCaptureDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var camera = videoCaptureDevices.FirstOrDefault();
            MediaCaptureInitializationSettings initialisationSettings = new MediaCaptureInitializationSettings()
            {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                VideoDeviceId = camera.Id
            };
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(initialisationSettings);
            _captureElement.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();
            var videoProperties = (_mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties);
            var videoSize = new Rect(0, 0, videoProperties.Width, videoProperties.Height);
            var detector = await FaceDetector.CreateAsync();
            var bitmap = FaceDetector.GetSupportedBitmapPixelFormats().First();
            try
            {
                await Task.Run(async () =>
                {
                    VideoFrame frame = new VideoFrame(bitmap, (int)videoSize.Width, (int)videoSize.Height);
                    TimeSpan? lastFrameTime = null;
                    while (true)
                    {
                        if (!_requestStopCancellationToken.Token.IsCancellationRequested)
                        {
                            await _mediaCapture.GetPreviewFrameAsync(frame);

                            if ((!lastFrameTime.HasValue) || (lastFrameTime != frame.RelativeTime))
                            {
                                var detectedFaces = await detector.DetectFacesAsync(frame.SoftwareBitmap);
                                if (detectedFaces.Count == 1)
                                {
                                    var convertedRgba16Bitmap = SoftwareBitmap.Convert(frame.SoftwareBitmap, BitmapPixelFormat.Rgba16);
                                    InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
                                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                                    encoder.SetSoftwareBitmap(convertedRgba16Bitmap);
                                    await encoder.FlushAsync();

                                    var detectedPersonId = await _faceService.DetectFace(stream.AsStream());

                                    if (detectedPersonId.HasValue)
                                    {
                                        _userService.PersonId = detectedPersonId.Value;
                                        var user = await _userService.GetModelAsync();
                                        if (user == null)
                                        {
                                            user = new UserProfileModel().RandomData();
                                            user.PersonId = detectedPersonId.Value;
                                            user = await _userService.AddUserAsync(user);
                                        }
                                        await UserViewModel.SetValuesAsync(User, user);
                                    }
                                    else
                                    {
                                        // bug: when a person was not detected, the stream gets disposed
                                        //stream.Seek(0);
                                        stream = new InMemoryRandomAccessStream();
                                        encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                                        encoder.SetSoftwareBitmap(convertedRgba16Bitmap);
                                        await encoder.FlushAsync();

                                        // TODO: ask new user for initial profile data
                                        var user = new UserProfileModel().RandomData();
                                        user.PersonId = await _faceService.CreatePersonAsync(user.FullName);
                                        var faceIds = new List<Guid>();
                                        faceIds.Add(await _faceService.AddFaceAsync(user.PersonId, stream.AsStream()));
                                        user.FaceIds = faceIds.ToArray();
                                        user = await _userService.AddUserAsync(user);
                                        await UserViewModel.SetValuesAsync(User, user);
                                    }

                                    await Task.Delay(10000, _requestStopCancellationToken.Token);
                                }
                            }
                            lastFrameTime = frame.RelativeTime;
                        }
                    }
                }, _requestStopCancellationToken.Token);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            await _mediaCapture.StopPreviewAsync();
            _captureElement.Source = null;
            _requestStopCancellationToken.Dispose();
        }

        internal void Start()
        {
            _requestStopCancellationToken = new CancellationTokenSource();
        }

        internal void Stop()
        {
            _requestStopCancellationToken.Cancel();
        }

        public void Dispose()
        {
            _requestStopCancellationToken.Cancel();
        }
    }
}