using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Flashcard
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        //Need to delete this
        [JsonProperty(PropertyName = "deck")]
        public Deck Deck { get; set; }

        [JsonProperty(PropertyName = "deckId")]
        public string DeckID { get; set; }

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }

        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        [JsonProperty(PropertyName = "tipsCount")]
        public int TipsCount { get; set; }

        //Need to delete this
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
