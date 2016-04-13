using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Models;

namespace StudyBox.Core.Messages
{
    public class SearchMessageToDeckList : MessageBase
    {
        public string SearchingContent { get; set; }

        public SearchMessageToDeckList(string searchingContent)
        {
            SearchingContent = searchingContent;
        }
    }
}
