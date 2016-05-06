using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Enums;

namespace StudyBox.Core.Messages
{
    public class DecksTypeMessage : MessageBase
    {
        public DecksType DecksType { get; set; }

        public DecksTypeMessage(DecksType decksType)
        {
            DecksType = decksType;
        }
    }
}
