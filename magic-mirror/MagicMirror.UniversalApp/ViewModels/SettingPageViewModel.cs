using MagicMirror.Business.Models;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Services;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private ILocationService _locationService;
        private ISettingsService _settingsService;
        private IAddressService _addressService;

        public SettingPageViewModel(ILocationService locationService, ISettingsService settingsService, IAddressService addressService)
        {
            _locationService = locationService;
            _settingsService = settingsService;
            _addressService = addressService;
        }

        public void SaveSettings()
        {
            _settingsService.SaveSettings(App.UserSettings);
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

        #endregion Properties
    }
}