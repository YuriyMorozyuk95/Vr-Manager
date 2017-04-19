using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VrManager.Convertors
{
    public class BoolToVisiblyConverter : IValueConverter
    {
     
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisibly = (bool)value;
            if(isVisibly)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
