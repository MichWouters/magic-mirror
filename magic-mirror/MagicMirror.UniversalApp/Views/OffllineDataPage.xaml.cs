using MagicMirror.UniversalApp.Common;
using MagicMirror.UniversalApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class OfflineDataPage : Page
    {
        private NavigationHelper navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public OfflineDataViewModel ViewModel { get; } = new OfflineDataViewModel();

        public OfflineDataPage()
        {
            DataContext = ViewModel;
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.SaveData(ViewModel);
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
    }
}