using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private Services.ISettingsService _settingService;
        private LocationService _locationService;
        private IApiService<AddressModel> _addressService;
        private UserSettings _userSettings;

        private string ipAddress;

        public SettingPageViewModel()
        {
            _settingService = new Services.SettingsService();
            _locationService = new LocationService();

            try
            {
                _userSettings = _settingService.LoadSettings();
            }
            catch (FileNotFoundException)
            {
                DisplayErrorMessage("No Settings File Found",
                    "It looks like you're running this app for the first time." +
                    " We created a new settings file with default values. Please enter your settings.");
            }
        }

        public void NavigateToMain()
        {
            _settingService.SaveSettings(_userSettings);
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

        #region Properties

        public string IpAddress
        {
            get => _settingService.GetIpAddress();
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