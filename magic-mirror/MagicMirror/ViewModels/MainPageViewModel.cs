using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using System.Diagnostics;
using System.Threading;
using MagicMirror.Business.Services;
using MagicMirror.Business.Services.Weather;
using MagicMirror.Business.Services.Traffic;
using MagicMirror.Business.Models.Weather;
using MagicMirror.Business.Models.Traffic;

namespace MagicMirror.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;

        public WeatherModel MagicMirrorDto { get; set; }

        public MainPageViewModel()
        {
            MagicMirrorDto = new WeatherModel();
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();
            Initialize();
        }

        private void Initialize()
        {
            var autoEvent = new AutoResetEvent(false);

            var timeTimer = new Timer(RefreshTime, autoEvent, 1000, 1000);
            var complimentTimer = new Timer(FillCompliment, autoEvent, 1000, 5000);
            var weatherTimer = new Timer(RefreshWeather, autoEvent, 1000, 20000);
            var trafficTimer = new Timer(RefreshTraffic, autoEvent, 1000, 10000);
        }

        private void RefreshWeather(Object stateInfo)
        {
            try
            {
                Dispatcher.Dispatch(() =>
                {
                    var weatherDailyDto = Task.Run(() => _weatherService.GetModelAsync()).Result;
                    //MagicMirrorDto.WeatherDaily = weatherDailyDto;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        private void RefreshTraffic(object stateInfo)
        {
            throw new NotImplementedException();
            //try
            //{
            //    Dispatcher.Dispatch(() =>
            //    {
            //        TrafficModel trafficModel = Task.Run(() => _trafficService.GetTrafficDtoAsync()).Result;
            //        MagicMirrorDto.Traffic = trafficModel;
            //    });

            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    throw;
            //}
        }

        private void FillCompliment(object stateInfo)
        {
            //Dispatcher.Dispatch(() =>
            //{
            //    string compliment = _weatherService.GenerateCompliment();
            //    MagicMirrorDto.Compliment = compliment;
            //});
        }

        private void RefreshTime(object stateInfo)
        {

            //Dispatcher.Dispatch(() =>
            //{
            //    MagicMirrorDto.Time = DateTime.Now.ToString("HH:mm:ss");
            //});

        }



        #region Navigation


        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {

            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        //public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        //public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        //public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        #endregion
    }
}