using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private ISettingsService _settingService;
        private ILocationService _locationService;
        private UserSettings _userSettings;

        private string ipAddress;

        public SettingPageViewModel()
        {
            _settingService = new SettingsService();
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

        public async Task GetLocation()
        {
            await _locationService.GetLocationAsync();
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