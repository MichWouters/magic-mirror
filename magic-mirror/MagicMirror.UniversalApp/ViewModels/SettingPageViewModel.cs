using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public UserSettings UserSettings
        {
            get
            {
                var appReference = Application.Current as App;
                return appReference.Criteria;
            }
            set
            {
                var appReference = Application.Current as App;

                appReference.Criteria = value;
                OnPropertyChanged();
            }
        }

        public async Task NavigateToMain()
        {
            try
            {
                SettingsService _service = new SettingsService();
                await _service.SaveSettings();

                var IAmTired = _service.ReadSettings();
                _navigationService.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Unable to save settings", ex.Message);
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}