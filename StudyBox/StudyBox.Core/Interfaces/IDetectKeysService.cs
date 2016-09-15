using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace StudyBox.Core.Interfaces
{
    public interface IDetectKeysService
    {
        void DetectKeyDown(KeyRoutedEventArgs e);
        void GotFocus(object obj);
    }
}
