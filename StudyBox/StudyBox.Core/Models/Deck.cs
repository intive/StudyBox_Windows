using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Deck
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isPublic")]
        public bool IsPublic { get; set; }

        //TODO connect with background as soon as will be possible
        public int CountOfFlashcards { get; set; }

        public Deck()
        {
            this.CountOfFlashcards = 0;
        }

        public Deck(string id)
        {
            this.ID = id;
            this.CountOfFlashcards = 0;
        }

        public Deck(string id, string name)
        {
            this.ID = id;
            this.Name = name;
            this.CountOfFlashcards = 0;
        }

        public Deck(string id, string name, int countOfFlashCards)
        {
            this.ID = id;
            this.Name = name;
            this.CountOfFlashcards = countOfFlashCards;
        }
    }
}
