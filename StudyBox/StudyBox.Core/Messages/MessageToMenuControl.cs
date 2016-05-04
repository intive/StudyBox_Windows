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
        public bool SaveButton { get; set; }
        public bool ExitButton { get; set; }
        public string TitleString { get; set; }

        public MessageToMenuControl(bool search, bool save, bool exit, string title="")
        {
            SearchButton = search;
            SaveButton = save;
            ExitButton = exit;
            TitleString = title;
        }

    }
}
