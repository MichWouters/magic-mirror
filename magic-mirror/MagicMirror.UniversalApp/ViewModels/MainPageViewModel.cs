using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Dto;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IService<WeatherModel> _weatherService;
        private readonly IService<TrafficModel> _trafficService;
        private SearchCriteria _searchCriteria;

        public event PropertyChangedEventHandler PropertyChanged;

        public MagicMirrorDto MagicMirrorDto { get; set; }

        public MainPageViewModel()
        {
            _searchCriteria = new SearchCriteria();

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
                WeatherModel result = Task.Run(() => _weatherService.GetModelAsync(_searchCriteria)).Result;
                return result;
            }
        }
        public TrafficModel Traffic => null;

        public string Compliment => "You look awful today";

        public string Time => DateTime.Now.ToString("HH:mm");
    }
}