using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Model
{
    public class Flashcard
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Flashcard() { }
        public Flashcard(string id, string question, string answer)
        {
            Id = id;
            Question = question;
            Answer = answer;
        }
    }
}
