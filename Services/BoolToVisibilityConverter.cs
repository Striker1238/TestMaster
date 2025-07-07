using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestMaster.Services
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;
            bool invert = parameter != null && parameter.ToString() == "False";
            if (invert)
                boolValue = !boolValue;
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                bool invert = parameter != null && parameter.ToString() == "False";
                return invert ? !result : result;
            }
            return false;
        }
    }
}