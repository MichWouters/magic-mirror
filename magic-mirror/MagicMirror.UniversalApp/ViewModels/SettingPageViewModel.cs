using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel : INotifyPropertyChanged
    {
        private SearchCriteria _searchCriteria;

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

        public void ToggleLightTheme()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
