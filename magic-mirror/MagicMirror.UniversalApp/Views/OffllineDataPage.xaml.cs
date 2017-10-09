using MagicMirror.UniversalApp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MagicMirror.UniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OffllineDataPage : Page
    {
        public OfflineDataViewModel ViewModel { get; } = new OfflineDataViewModel();

        public OffllineDataPage()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.SaveData(ViewModel);
        }
    }
}
