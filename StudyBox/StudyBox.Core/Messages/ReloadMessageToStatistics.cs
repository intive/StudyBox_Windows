using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace StudyBox.Core.Messages
{
    public class ReloadMessageToStatistics : MessageBase
    {
        public bool Reload { get; set; }

        public ReloadMessageToStatistics(bool reload=true)
        {
            Reload = reload;
        }
    }
}
