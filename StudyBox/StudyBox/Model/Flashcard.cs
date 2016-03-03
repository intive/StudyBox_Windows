using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Model
{
    public class Flashcard
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "deck")]
        public Deck Deck { get; set; }

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }

        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        [JsonProperty(PropertyName = "hint")]
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
