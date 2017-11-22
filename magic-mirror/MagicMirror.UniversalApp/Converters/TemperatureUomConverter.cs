using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MagicMirror.UniversalApp.Converters
{
    public class UomEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value as ComboBoxItem;
        }
    }
}