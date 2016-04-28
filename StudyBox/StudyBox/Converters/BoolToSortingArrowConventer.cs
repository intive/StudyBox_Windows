using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace StudyBox.Converters
{
    public class BoolToSortingArrowConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    if ((bool) value)
                        return Symbol.Upload;
                    else
                        return Symbol.Download;
                }
            }
            return Symbol.Sort;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
