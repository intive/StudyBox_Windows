using Newtonsoft.Json;

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

        public Flashcard() { }

        public Flashcard(string id, string deckId, string question, string answer, bool idHidden, int tipsCount)
        {
            Id = id;
            DeckID = deckId;
            Question = question;
            Answer = answer;
            IsHidden = IsHidden;
            TipsCount = tipsCount;
        }
    }
}
