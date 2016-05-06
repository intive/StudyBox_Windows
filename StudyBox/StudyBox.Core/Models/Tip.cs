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

        public Tip()
        {
            Difficult = 1;
        }

        public Tip(string id, string essence)
        {
            this.ID = id;
            this.Essence = essence;
            Difficult = 1;
        }
    }
}
