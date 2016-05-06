using Newtonsoft.Json;
using System.Collections.Generic;

namespace StudyBox.Core.Models
{
    public class Flashcard
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "deckId")]
        public string DeckID { get; set; }

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }

        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        [JsonProperty(PropertyName = "isHidden")]
        public bool IsHidden { get; set; }

        [JsonProperty(PropertyName = "tipsCount")]
        public int TipsCount { get; set; }

        public List<Tip> Tips { get; set; }

        public Flashcard()
        {
            Tips = new List<Tip>();
        }

        public Flashcard(string id, string deckId, string question, string answer, bool idHidden, int tipsCount)
        {
            Id = id;
            DeckID = deckId;
            Question = question;
            Answer = answer;
            IsHidden = IsHidden;
            TipsCount = tipsCount;
            Tips = new List<Tip>();
        }
    }
}
