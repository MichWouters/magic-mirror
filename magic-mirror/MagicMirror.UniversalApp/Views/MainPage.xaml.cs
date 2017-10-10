using MagicMirror.UniversalApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; } = new MainPageViewModel();

        public MainPage()
        {
            DataContext = ViewModel;
            InitializeComponent();

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateToSettings();
        }

        private void TemperatureTextBlock_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.NavigateToOfflineData();
        }
    }
}