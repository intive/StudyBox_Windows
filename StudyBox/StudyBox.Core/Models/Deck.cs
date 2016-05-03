using Newtonsoft.Json;
using System;

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

        [JsonProperty(PropertyName = "flashcardsCount")]
        public int CountOfFlashcards { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(PropertyName = "creatorEmail")]
        public string CreatorEmail { get; set; }

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
