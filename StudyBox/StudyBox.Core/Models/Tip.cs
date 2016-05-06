using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Tip
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "essence")]
        public string Essence { get; set; }

        [JsonProperty(PropertyName = "difficult")]
        public int Difficult { get; set; }

        public Tip() { }

        public Tip(string id, string essence)
        {
            this.ID = id;
            this.Essence = essence;
        }
    }
}
