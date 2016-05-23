using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Messages
{
    public class MessageToChangePassword
    {
        public bool ChangePassword { get; set; }

        public MessageToChangePassword(bool changePassword)
        {
            ChangePassword = changePassword;
        }
    }
}
