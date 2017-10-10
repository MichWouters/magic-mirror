using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagicMirror.Business.Models
{
    /// <summary>
    /// Helper class to pass user parameters from the business to the data-layer.
    /// </summary>
    public class UserSettings : INotifyPropertyChanged
    {
        #region Constructors

        public UserSettings()
        {
            UserName = "Unknown User";
            HomeAddress = "709 Honey Creek ";
            HomeCity = "New York, NY 10028";
            WorkAddress = "3 South Sherman Street Astoria, NY 11106";

            Precision = 2;
            TemperatureUOM = TemperatureUOM.Celsius;
            DistanceUOM = DistanceUOM.Metric;
        }

        // Empty constructor chaining
        public UserSettings(string userName, string homeAddress, string homeCity, string workAddress)
            : this()
        {
            UserName = userName;
            HomeAddress = homeAddress;
            HomeCity = homeCity;
            WorkAddress = workAddress;
        }

        // Constructor chaining with parameters
        public UserSettings(string userName, string homeAddress, string homeCity, string workAddress, int precison, TemperatureUOM temperatureUOM, DistanceUOM distanceUOM)
            : this(userName, homeAddress, homeCity, workAddress)
        {
            Precision = precison;
            TemperatureUOM = temperatureUOM;
            DistanceUOM = distanceUOM;
        }

        #endregion Constructors

        #region Properties

        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _homeCity;

        public string HomeCity
        {
            get => _homeCity;
            set
            {
                _homeCity = value;
                OnPropertyChanged();
            }
        }

        private string _homeAddress;

        public string HomeAddress
        {
            get => _homeAddress;
            set
            {
                _homeAddress = value;
                OnPropertyChanged();
            }
        }

        private string _workAddress;

        public string WorkAddress
        {
            get => _workAddress;
            set
            {
                _workAddress = value;
                OnPropertyChanged();
            }
        }

        private int _precision;

        public int Precision
        {
            get => _precision;
            set
            {
                _precision = value;
                OnPropertyChanged();
            }
        }

        private TemperatureUOM _temperatureUOM;

        public TemperatureUOM TemperatureUOM
        {
            get => _temperatureUOM;
            set
            {
                _temperatureUOM = value;
                OnPropertyChanged();
            }
        }

        private DistanceUOM _distanceUOM;

        public DistanceUOM DistanceUOM
        {
            get => _distanceUOM;
            set
            {
                _distanceUOM = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}