using MagicMirror.Business.Models;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private UserSettings _userSettings;
        private Services.ILocationService _locationService;
        private Services.ISettingsService _settingsService;
        private IService<AddressModel> _addressService;

        public SettingPageViewModel(Services.ISettingsService settingsService)
        {
            _settingsService = settingsService;

            try
            {
                _userSettings = _settingsService.LoadSettings();
            }
            catch (FileNotFoundException ex)
            {
                DisplayErrorMessage("Settings file not found", ex.Message);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to set up SettingPageViewModel", ex.Message);
            }
        }

        public void SaveSettings()
        {
            _settingsService.SaveSettings(_userSettings);
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
                // TODO: Modernize
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
                DisplayErrorMessage("Unable to fetch location, Unauthorized access", e.Message);
                return null;
            }
            catch (Exception e)
            {
                DisplayErrorMessage("Unable to fetch location", e.Message);
                return null;
            }
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
                NotifyPropertyChanged();
            }
        }

        public UserSettings UserSettings
        {
            get => _userSettings;
            set
            {
                _userSettings = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Properties
    }
}