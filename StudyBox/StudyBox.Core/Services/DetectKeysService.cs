using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace StudyBox.Core.Services
{
    public class DetectKeysService : IDetectKeysService
    {
        public void GotFocus(object obj)
        {
            
            FrameworkElement focus = FocusManager.GetFocusedElement() as FrameworkElement;
            if (focus != null)
            {
                Debug.WriteLine("got focus: " + focus.Name + " (" + focus.GetType().ToString() + ")");
            }
            if (focus.GetType() == typeof(GridViewItem))
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
            }
        }

        public void DetectKeyDown(KeyRoutedEventArgs e)
        {
            switch (e.OriginalKey)
            {
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Down);
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Up);
                    break;

                case Windows.System.VirtualKey.Escape:
                case Windows.System.VirtualKey.GamepadMenu:
                    CoreApplication.Exit();
                    break;

                case Windows.System.VirtualKey.F:
                case Windows.System.VirtualKey.GamepadA:
                    //if (SearchOpacity == 0)
                        //OpenMenu();
                    break;

                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Left);
                    break;

                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Right);
                    break;
            }
        }
    }
}
