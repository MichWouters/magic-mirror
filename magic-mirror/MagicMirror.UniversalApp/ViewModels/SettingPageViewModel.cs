using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.IO;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private SettingsService _settingService;
        private UserSettings _userSettings;

        public SettingPageViewModel()
        {
            _settingService = new SettingsService();

            try
            {
                _userSettings = _settingService.LoadSettings();
            }
            catch (FileNotFoundException)
            {
                DisplayErrorMessage("No Settings File Found", "It looks like you're running this app for the first time. We created a new settings file with default values. Please enter your settings.");
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

        #region Properties

        private string _ipAddress;

        public string IpAddress
        {
            get => _settingService.GetIpAddress();
            set
            {
                _ipAddress = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}