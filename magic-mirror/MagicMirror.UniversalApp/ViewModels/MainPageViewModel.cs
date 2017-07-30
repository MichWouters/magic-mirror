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

        public MagicMirrorDto MagicMirrorDto { get; set; }

        public MainPageViewModel()
        {
            MagicMirrorDto = new MagicMirrorDto();
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();

            Initialize();
        }

        private void Initialize()
        {
            MagicMirrorDto.WeatherModel = Weather;
            MagicMirrorDto.TrafficModel = Traffic;
            MagicMirrorDto.Time = Time;
            MagicMirrorDto.Compliment = Compliment;
        }

        public WeatherModel Weather
        {
            get
            {
                WeatherModel result = Task.Run(() => _weatherService.GetModelAsync()).Result;
                return result;
            }
        }

        public TrafficModel Traffic
        {
            get
            {
                TrafficModel result = Task.Run(() => _trafficService.GetModelAsync()).Result;
                return result;
            }
        }

        public string Compliment
        {
            get
            {
                return "You look nice today";
            }
        }

        public string Time
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }
    }
}