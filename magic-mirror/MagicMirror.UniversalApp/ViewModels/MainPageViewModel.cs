using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Dto;
using System;
using System.Threading.Tasks;

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
            Dto.WeatherModel = RefreshWeather;
            //Dto.TrafficModel = RefreshTraffic();
            Dto.Time = RefreshTime();
        }

        public WeatherModel RefreshWeather
        {
            get
            {
                WeatherModel result = Task.Run(() => _weatherService.GetModelAsync()).Result;
                return result;
            }
        }

        private TrafficModel RefreshTraffic()
        {
            TrafficModel result = Task.Run(() => _trafficService.GetModelAsync()).Result;
            return result;
        }

        private string RefreshCompliment()
        {
            throw new NotImplementedException();
        }

        private string RefreshTime()
        {
            return DateTime.Now.ToString("HH:mm");
        }
    }
}