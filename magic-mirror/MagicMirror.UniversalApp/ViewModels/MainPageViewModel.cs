using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Dto;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;

        public MagicMirrorDto dto { get; set; }

        public MainPageViewModel()
        {
            dto = new MagicMirrorDto();
            _weatherService = new WeatherService();
            _trafficService = new TrafficService();
        }
    }
}
