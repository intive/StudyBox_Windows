using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace StudyBox.Core.Messages
{
    public class MessageToMenuControl : MessageBase
    {
        public bool SearchButton { get; set; }
        public bool EditButton { get; set; }
        public bool SaveButton { get; set; }
        public bool ExitButton { get; set; }

        public MessageToMenuControl(bool search, bool edit, bool save, bool exit)
        {
            SearchButton = search;
            EditButton = edit;
            SaveButton = save;
            ExitButton = exit;
        }
    }
}
