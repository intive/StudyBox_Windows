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
        public bool RemoveButton { get; private set; }
        public bool NoRemoveButton { get; private set; }
        public bool SettingsButton { get; private set; }
        public bool OkButton { get; private set; }
        public string Message { get; set; }



        public MessageToMessageBoxControl(bool visiblity, bool loginButton = false, string message = "")
        {
            Visibility = visiblity;
            Message = message;
            LoginButton = loginButton;
        }

        public MessageToMessageBoxControl(bool visiblity, bool removeButton, bool noRemoveButton, string message = "")
        {
            Visibility = visiblity;
            Message = message;
            RemoveButton = removeButton;
            NoRemoveButton = noRemoveButton;
        }

        public MessageToMessageBoxControl(bool visiblity, bool loginButton, bool okButton = false, bool settingsButton = false, string message = "")
        {
            Visibility = visiblity;
            Message = message;
            LoginButton = loginButton;
            SettingsButton = settingsButton;
            OkButton = okButton;
        }
    }
}
