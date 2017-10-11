using MagicMirror.Business.Models;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

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

        public async Task<FetchedAddress> GetAddressModel()
        {
            try
            {
                var coordinates = await _locationService.GetLocationAsync();
                _addressService = new AddressService(coordinates.Coordinate.Latitude.ToString(), coordinates.Coordinate.Longitude.ToString());
                AddressModel addressModel = await _addressService.GetModelAsync();

                string address = $"{addressModel.Street}, {addressModel.HouseNumber}";
                string city = $"{addressModel.PostalCode} {addressModel.City}, {addressModel.Country}";

                UserSettings.HomeAddress = address;
                UserSettings.HomeCity = city;
                return new FetchedAddress { Address = address, City = city };
            }
            catch (UnauthorizedAccessException e)
            {
                DisplayErrorMessage("Unable to fetch location", e.Message);
                return null;
            }
            catch (Exception e)
            {
                DisplayErrorMessage("Unable to fetch location", e.Message);
                return null;
            }
        }

        public void SaveSettings(bool createNewSettings = false)
        {
            try
            {
                if (createNewSettings)
                {
                    _userSettings = new UserSettings();
                    DisplayErrorMessage("Default settings created", "No settings file found! Creating a new one with default settings");
                }

                if (_userSettings != null)
                {
                    _settingsService.SaveSettings(localFolder, SETTING_FILE, _userSettings);
                    NavigateToMain();
                }
                else
                    DisplayErrorMessage("Unable to save Settings. Please check your input");
            }
            catch (Exception e)
            {
                DisplayErrorMessage("Unable to save Settings.", e.Message);
            }
        }

        private UserSettings LoadSettings()
        {
            try
            {
                var result = _settingsService.ReadSettings(localFolder, SETTING_FILE);

                if (result == null) throw new FileNotFoundException();

                return result;
            }
            catch (FileNotFoundException)
            {
                SaveSettings(true);
                _userSettings = LoadSettings();
                throw;
            }
            catch (Exception) { throw; }
        }

        private string GetIpAddress()
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