using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace StudyBox.Core.Messages
{
    public class ReloadMessageToDecksList : MessageBase
    {
        public bool Reload { get; set; }

        public ReloadMessageToDecksList(bool b)
        {
            Reload = b;
        }
    }
}
