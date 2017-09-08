using System;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.ViewModels
{
    public abstract class ViewModelBase
    {
        private bool _contentDialogShown;

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
    }
}