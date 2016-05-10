using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Messages
{
    public class MessageToMessageBoxControl : MessageBase
    {
        public bool Visibility { get; set; }
        public bool LoginButton { get; set; }
        public string Message { get; set; }
        public MessageToMessageBoxControl(bool visiblity, bool loginButton = false, string message = "")
        {
            Visibility = visiblity;
            Message = message;
            LoginButton = loginButton;
        }
    }
}
