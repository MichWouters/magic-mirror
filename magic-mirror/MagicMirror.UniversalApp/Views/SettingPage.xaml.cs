using MagicMirror.Business.Models;
using System;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
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
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}