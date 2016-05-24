using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace StudyBox.Converters
{
    public class NegativeBooleanToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is bool && (bool)value == false;
        }
    }
}
