using Windows.UI.Xaml.Controls;
using MagicMirror.UniversalApp.ViewModels;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class SettingPage : Page
    {
        public SettingPageViewModel ViewModel { get; } = new SettingPageViewModel();

        public SettingPage()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}