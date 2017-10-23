using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.Common
{
    // Sealed class cannot be inherited from
    public sealed class NavigationService : INavigationService
    {
        private readonly Frame frame;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);
            return Navigate(type, parameter);
        }

        public bool Navigate(Type t, object parameter = null)
        {
            return frame.Navigate(t, parameter);
        }

        public void GoBack()
        {
            frame.GoBack();
        }
    }
}