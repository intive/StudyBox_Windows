using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace StudyBox.Core.Interfaces
{
    public interface IGravatarService
    {
        string GetUserGravatarUrl(string email);
        string GetDefaultGravatarUrl();
    }
}
