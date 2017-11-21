namespace MagicMirror.UniversalApp.ViewModels
{
    public class ViewModelLocator
    {
        private static MainPageViewModel mainPageViewModel = new MainPageViewModel();
        private static SettingPageViewModel settingPageViewModel = new SettingPageViewModel();

        public static MainPageViewModel MainPageViewModel
        {
            get { return mainPageViewModel; }
        }

        public static SettingPageViewModel SettingPageViewModel
        {
            get { return settingPageViewModel; }
        }
    }
}