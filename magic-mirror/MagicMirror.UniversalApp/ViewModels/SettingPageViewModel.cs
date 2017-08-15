using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class SettingPageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
