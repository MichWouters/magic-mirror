using MagicMirror.UniversalApp.Services;
using System;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;

namespace MagicMirror.UniversalApp.ViewModels
{
    /// <summary>
    /// Base class for ViewModels. Provides INotifPropertyChanged, navigation and error handling functionality
    /// </summary>
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        private bool _contentDialogShown;
        protected NavigationService _navigationService;

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

        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}