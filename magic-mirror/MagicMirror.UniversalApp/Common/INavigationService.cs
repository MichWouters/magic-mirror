using System;

namespace MagicMirror.UniversalApp.Common
{
    public interface INavigationService
    {
        void GoBack();

        bool Navigate(Type t, object parameter = null);

        bool Navigate<T>(object parameter = null);
    }
}