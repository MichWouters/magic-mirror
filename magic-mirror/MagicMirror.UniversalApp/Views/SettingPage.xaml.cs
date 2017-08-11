using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var str = loader.GetString("Name");
        }
    }
}
