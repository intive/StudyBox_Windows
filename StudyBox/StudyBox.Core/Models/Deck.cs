using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Deck
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        public Deck() { }

        public Deck(string id)
        {
            this.ID = id;
        }

        public Deck(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
