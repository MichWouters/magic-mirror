using System.ComponentModel;
using System.Runtime.CompilerServices;
using MagicMirror.Business.Models;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel:INotifyPropertyChanged
    {
        private SearchCriteria _searchCriteria;

        public SearchCriteria SearchCriteria
        {
            get => _searchCriteria;
            set
            {
                _searchCriteria = value; 
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
