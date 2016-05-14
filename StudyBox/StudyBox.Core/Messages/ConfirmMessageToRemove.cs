using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace StudyBox.Core.Messages
{
    public class ConfirmMessageToRemove : MessageBase
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmMessageToRemove(bool shouldBeRemoved)
        {
            IsConfirmed = shouldBeRemoved;
        }
    }
}
