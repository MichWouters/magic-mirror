using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private LocationService _locationService;
        private UserSettings _userSettings;
        private ISettingsService _settingsService;
        private IApiService<AddressModel> _addressService;



        public SettingPageViewModel()
        {
            _settingsService = new SettingsService();
            _locationService = new LocationService();

            try
            {
                _userSettings = LoadSettings();
            }
            catch (FileNotFoundException)
            {
                DisplayErrorMessage("No Settings File Found",
                    "It looks like you're running this app for the first time." +
                    " We created a new settings file with default values. Please enter your settings now.");
            }
        }

        public void NavigateToMain()
        {
            SaveSettings(localFolder, SETTING_FILE, _userSettings);
            _navigationService.Navigate(typeof(MainPage));
        }

        public void ToggleLightTheme()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Cannot switch theme at this time", ex.Message);
            }
        }

        public async Task<object> GetAddressModel()
        {
            try
            {
                var coordinates = await _locationService.GetLocationAsync();
                _addressService = new AddressService(coordinates.Coordinate.Latitude.ToString(), coordinates.Coordinate.Longitude.ToString());
                AddressModel addressModel = await _addressService.GetModelAsync();

                string address = $"{addressModel.Street}, {addressModel.HouseNumber}";
                string city = $"{addressModel.PostalCode} {addressModel.City}, {addressModel.Country}";

                // Todo: Convert anonymous type to strongly typed
                UserSettings.HomeAddress = address;
                UserSettings.HomeCity = city;
                return new { Address = address, City = city };
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to fetch location", ex.Message);
                return null;
            }
        }

        private void SaveSettings(string path, string fileName, UserSettings settings)
        {
            _settingsService.SaveSettings(path, fileName, settings);
        }

        public UserSettings LoadSettings()
        {
            try
            {
                var result = _settingsService.ReadSettings(localFolder, SETTING_FILE);

                if (result == null) throw new FileNotFoundException();

                return result;
            }
            catch (FileNotFoundException)
            {
                SaveSettings(localFolder, SETTING_FILE, new UserSettings());
                throw;
            }
            catch (Exception) { throw; }
        }

        public string GetIpAddress()
        {
            try
            {
                string result = "";
                foreach (Windows.Networking.HostName localHostName in NetworkInformation.GetHostNames())
                {
                    if (localHostName.IPInformation != null)
                    {
                        if (localHostName.Type == Windows.Networking.HostNameType.Ipv4)
                        {
                            result = localHostName.ToString();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(result)) throw new Exception();
                return result;
            }
            catch (Exception)
            {
                return "Unable to retrieve IP Address";
            }
        }

        #region Properties

        private string ipAddress;
        public string IpAddress
        {
            get => GetIpAddress();
            set
            {
                ipAddress = value;
                OnPropertyChanged();
            }
        }

        public UserSettings UserSettings
        {
            get => _userSettings;
            set
            {
                _userSettings = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}