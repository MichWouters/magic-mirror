using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class CameraFeedViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        protected CancellationTokenSource _requestStopCancellationToken;
        protected CaptureElement _captureElement;
        protected MediaCapture _mediaCapture;
        protected Queue<Stream> _pictureQueue;
        public CaptureElement CaptureElement
        {
            get
            {
                return _captureElement;
            }
        }

        public CameraFeedViewModel()
        {
            _pictureQueue = new Queue<Stream>();
            InitializeCamera();
        }

        private async void InitializeCamera()
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
                                var convertedRgba16Bitmap = SoftwareBitmap.Convert(frame.SoftwareBitmap, BitmapPixelFormat.Rgba16);
                                InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
                                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                                encoder.SetSoftwareBitmap(convertedRgba16Bitmap);
                                await encoder.FlushAsync();
                                _pictureQueue.Enqueue(stream.AsStream());
                                if (_pictureQueue.Count > 2)
                                {
                                    _pictureQueue.Dequeue();
                                }
                                await Task.Delay(60000, _requestStopCancellationToken.Token);
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

        public void Dispose()
        {
            _requestStopCancellationToken.Cancel();
        }
    }
}