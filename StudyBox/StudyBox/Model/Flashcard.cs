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
        public Deck Deck { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Hint { get; set; }

        public Flashcard() { }
        public Flashcard(string id, Deck deck, string question, string answer, string hint)
        {
            Id = id;
            Deck = deck;
            Question = question;
            Answer = answer;
            Hint = hint;
        }
    }
}
