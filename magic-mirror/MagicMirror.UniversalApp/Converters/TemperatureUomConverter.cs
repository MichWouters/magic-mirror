using MagicMirror.Business.Models;
using System;
using Windows.UI.Xaml.Data;

namespace MagicMirror.UniversalApp.Converters
{
    public class UomEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int result = (int)value;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}