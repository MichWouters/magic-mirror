using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.ViewModels;
using Windows.UI.Xaml.Controls;
using System;

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

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.NavigateToMain();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FillDropDownLists();
        }

        private void FillDropDownLists()
        {
            TemperatureUomComboBox.ItemsSource = Enum.GetValues(typeof(TemperatureUOM));
            TemperatureUomComboBox.SelectedIndex = (int)ViewModel.UserSettings.TemperatureUOM;

            DistanceUomComboBox.ItemsSource = Enum.GetValues(typeof(DistanceUOM));
            DistanceUomComboBox.SelectedIndex = (int)ViewModel.UserSettings.DistanceUOM;
        }
    }
}