using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.ViewModels
{
    /// <summary>
    /// Base class for ViewModels. Provides INotifPropertyChanged, navigation and error handling functionality
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _contentDialogShown;
        protected NavigationService _navigationService;

        protected readonly string localFolder = ApplicationData.Current.LocalFolder.Path;
        protected const string SETTING_FILE = "settings.json";

        public ViewModelBase()
        {
            _navigationService = new NavigationService();
        }

        protected async void DisplayErrorMessage(string title, string content = "")
        {
            // Only one dialog can be open
            if (!_contentDialogShown)
            {
                _contentDialogShown = true;
                var errorMessage = new ContentDialog
                {
                    Title = title,
                    Content = content,
                    PrimaryButtonText = "Ok"
                };
                ContentDialogResult result = await errorMessage.ShowAsync();
                _contentDialogShown = false;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Navigation

        public void NavigateToMain()
        {
            try
            {
                _navigationService.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Offline Data", ex.Message);
            }
        }

        public void NavigateToSettings()
        {
            try
            {
                _navigationService.Navigate(typeof(SettingPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Settings", ex.Message);
            }
        }

        public void NavigateToOfflineData()
        {
            try
            {
                _navigationService.Navigate(typeof(OffllineDataPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to navigate to Offline Data", ex.Message);
            }
        }

        #endregion Navigation
    }
}