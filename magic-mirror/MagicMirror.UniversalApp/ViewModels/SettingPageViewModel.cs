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

        public async Task GetAddressModel()
        {
            var pos = await _locationService.GetLocationAsync();


            _addressService = new Services.

            AddressModel addressModel =  await _addressService.GetModelAsync();

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