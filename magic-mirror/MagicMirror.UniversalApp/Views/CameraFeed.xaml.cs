using MagicMirror.UniversalApp.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class CameraFeed : UserControl
    {
        public CameraFeedViewModel ViewModel { get; } = new CameraFeedViewModel();

        public CameraFeed()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}