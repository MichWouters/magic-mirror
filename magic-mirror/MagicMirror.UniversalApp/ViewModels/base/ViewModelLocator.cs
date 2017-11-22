using GalaSoft.MvvmLight.Ioc;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class ViewModelLocator
    {
        public static MainPageViewModel MainPageViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainPageViewModel>();
            }
            set
            {
            }
        }

        public static SettingPageViewModel SettingPageViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SettingPageViewModel>();
            }
            set
            {
            }
        }
    }
}