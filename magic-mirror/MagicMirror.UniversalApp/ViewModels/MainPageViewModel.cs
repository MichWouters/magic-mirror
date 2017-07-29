using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Dto;
using System;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;

        public MagicMirrorDto Dto { get; set; }

        public MainPageViewModel()
        {
            Dto = new MagicMirrorDto();
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();

            Initialize();
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }

        private WeatherModel RefreshWeather
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private TrafficModel RefreshTraffic()
        {
            throw new NotImplementedException();
        }

        private string RefreshCompliment()
        {
            throw new NotImplementedException();
        }

        private void RefreshTime()
        {
        }
    }
}