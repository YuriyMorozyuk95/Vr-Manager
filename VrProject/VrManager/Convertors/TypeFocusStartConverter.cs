using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VrManager.Data.Abstract;

namespace VrManager.Convertors
{
    class TypeFocusStartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TypeStartFocus type = (TypeStartFocus)value;
            switch(type)
            {
                case TypeStartFocus.FocusedInFullScreen:
                    return "Запуск с переключениям в полноекранний режым";
                case TypeStartFocus.FocusToMainWnd:
                    return "Запуск с переключения фокуса на Vr Manager";
                case TypeStartFocus.FocusToTitle:
                    return ">Запуск с переключения фокуса заголовок окна";
                default:
                    return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
