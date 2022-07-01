using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace thing_list
{

        public class Convert_size : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Math.Round((double)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }
        }

}
