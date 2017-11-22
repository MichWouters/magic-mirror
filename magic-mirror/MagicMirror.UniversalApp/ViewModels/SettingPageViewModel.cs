using MagicMirror.Business.Models;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Common;
using MagicMirror.UniversalApp.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private ILocationService _locationService;
        private IAddressService _addressService;

        private ISettingsService _settingsService;
        private ICommonService _commonService;

        private string name;
        private string homeAddress;
        private string homeTown;
        private string workAddress;
        private int decimals;
        private DistanceUOM distanceUom;
        private TemperatureUOM temperatureUOM;

        public ICommand SaveSettingsCommand { get; set; }
        public ICommand GetUserLocationCommand { get; set; }

        public SettingPageViewModel(ISettingsService settingsService, ICommonService commonService, ILocationService locationService, IAddressService addressService)
        {
            _settingsService = settingsService;
            _commonService = commonService;
            _locationService = locationService;
            _addressService = addressService;

            SaveSettingsCommand = new CustomCommand(SaveSettings, CanSaveSettings);
            GetUserLocationCommand = new CustomCommand(GetuserLocationAsync, CanGetUserLocation);

            FillSettingsInformation();
        }

        private bool CanGetUserLocation(object obj)
        {
            return true;
        }

        private bool CanSaveSettings(object obj)
        {
            return true;
        }

        private async void GetuserLocationAsync(object obj)
        {
            var address = await GetAddressModel();
            HomeAddress = address.Address;
            HomeTown = address.City;
        }

        private void SaveSettings(object obj)
        {
            var usersettings = App.UserSettings;
            usersettings.DistanceUOM = DistanceUom;
            usersettings.HomeAddress = HomeAddress;
            usersettings.HomeCity = HomeTown;
            usersettings.TemperatureUOM = temperatureUOM;
            usersettings.UserName = Name;
            usersettings.WorkAddress = WorkAddress;

            _settingsService.SaveSettings(usersettings);
        }

        private void FillSettingsInformation()
        {
            var settings = App.UserSettings;

            Name = "Unknown";
            homeAddress = settings.HomeAddress;
            HomeTown = settings.HomeCity;
            WorkAddress = settings.WorkAddress;
            Decimals = settings.Precision;
            DistanceUom = settings.DistanceUOM;
            TemperatureUOM = settings.TemperatureUOM;
        }

        public async Task<FetchedAddress> GetAddressModel()
        {
            try
            {
                // TODO: Modernize
                var coordinates = await _locationService.GetLocationAsync();
                _addressService = new AddressService();
                AddressModel addressModel = await _addressService.GetModelAsync(coordinates.Coordinate.Latitude.ToString(), coordinates.Coordinate.Longitude.ToString());

                string address = $"{addressModel.Street}, {addressModel.HouseNumber}";
                string city = $"{addressModel.PostalCode} {addressModel.City}, {addressModel.Country}";

                App.UserSettings.HomeAddress = address;
                App.UserSettings.HomeCity = city;
                return new FetchedAddress { Address = address, City = city };
            }
            catch (UnauthorizedAccessException e)
            {
                DisplayErrorMessage("Unable to fetch location, Unauthorized access", e.Message);
                return null;
            }
            catch (Exception e)
            {
                DisplayErrorMessage("Unable to fetch location", e.Message);
                return null;
            }
        }

        #region Properties

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string HomeAddress
        {
            get { return homeAddress; }
            set
            {
                homeAddress = value;
                NotifyPropertyChanged();
            }
        }

        public string HomeTown
        {
            get { return homeTown; }
            set
            {
                homeTown = value;
                NotifyPropertyChanged();
            }
        }

        public string WorkAddress
        {
            get { return workAddress; }
            set
            {
                workAddress = value;
                NotifyPropertyChanged();
            }
        }

        public int Decimals
        {
            get { return decimals; }
            set
            {
                decimals = value; NotifyPropertyChanged();
            }
        }

        public DistanceUOM DistanceUom
        {
            get { return distanceUom; }
            set
            {
                distanceUom = value;
                NotifyPropertyChanged();
            }
        }

        public TemperatureUOM TemperatureUOM
        {
            get { return temperatureUOM; }
            set
            {
                temperatureUOM = value;
                NotifyPropertyChanged();
            }
        }

        private string ipAddress;

        public string IpAddress
        {
            get => _commonService.GetIpAddress();
            set
            {
                ipAddress = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Properties
    }
}