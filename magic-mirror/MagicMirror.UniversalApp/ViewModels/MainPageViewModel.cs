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

        public WeatherModel Weather => null;

        public TrafficModel Traffic => null;

        public string Compliment => "You look nice today";

        public string Time => DateTime.Now.ToString("HH:mm");
    }
}