using MagicMirror.Business.Models;
using MagicMirror.UniversalApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

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
            ViewModel.NavigateToMain();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FillDropDownLists();
        }

        private void FillDropDownLists()
        {
            try
            {
                TemperatureUomComboBox.ItemsSource = Enum.GetValues(typeof(TemperatureUOM));
                DistanceUomComboBox.ItemsSource = Enum.GetValues(typeof(DistanceUOM));
                TemperatureUomComboBox.SelectedIndex = (int)ViewModel.UserSettings.TemperatureUOM;
                DistanceUomComboBox.SelectedIndex = (int)ViewModel.UserSettings.DistanceUOM;
            }
            catch (Exception e)
            {
            }
        }
    }
}