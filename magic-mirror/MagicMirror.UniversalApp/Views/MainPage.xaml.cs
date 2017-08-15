using MagicMirror.UniversalApp.ViewModels;
using Windows.UI.Xaml.Controls;

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
    }
}
