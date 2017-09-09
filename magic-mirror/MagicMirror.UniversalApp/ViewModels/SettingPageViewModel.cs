using MagicMirror.Business.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public SearchCriteria SearchCriteria
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

        internal void SaveSettings()
        {
            throw new NotImplementedException();
        }

        public void ToggleLightTheme()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
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