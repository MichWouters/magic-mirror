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

        public WeatherModel WeatherModel { get; set; }
        public TrafficModel TrafficModel { get; set; }


        public MainPageViewModel()
        {
            _searchCriteria = new SearchCriteria
            {
                City = "Houwaart",
                Destination = "Generaal ArmstrongWeg 1, Antwerpen",
                Start = "Heikant 51 Houwaart",
                UserName = "Michiel"
            };

            _weatherService = new WeatherService();
            _trafficService = new TrafficService();

            Initialize();
        }

        private void Initialize()
        {
            this.WeatherModel = Weather;
            this.TrafficModel = Traffic;
            this.Compliment = Compliment;
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

        private string compliment;

        public string Compliment
        {
            get { return "You look awful today"; }
            set { compliment = value; }
        }


        public string Time
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}