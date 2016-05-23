using StudyBox.Core.Models;
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
        public ResetPassword ReturnToken { get; set; }
        public string Email { get; set; }

        public MessageToChangePassword(bool changePassword, ResetPassword resetPassword, string email)
        {
            ChangePassword = changePassword;
            ReturnToken = resetPassword;
            Email = email;
        }
    }
}
