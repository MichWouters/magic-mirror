using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagicMirror.Business.Models
{
    /// <summary>
    /// Helper class to pass user parameters from the business to the data-layer.
    /// </summary>
    public class UserSettings: INotifyPropertyChanged
    {
        public UserSettings()
        {
            UserName = "Billy";
            HomeAddress = "Generaal Armstrongweg 1";
            HomeCity = "Antwerpen";
            WorkAddress = "Earl Bakkenstraat 10 6422 Heerlen";

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

        public string UserName { get; set; }
        public string HomeCity { get; set; }
        public string HomeAddress { get; set; }
        public string WorkAddress { get; set; }
        public int Precision { get; set; }
        public TemperatureUOM TemperatureUOM { get; set; }
        public DistanceUOM DistanceUOM { get; set; }

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}