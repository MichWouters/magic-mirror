using MagicMirror.UniversalApp.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.ViewModels
{
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}