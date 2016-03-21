using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Tag
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public Tag() { }

        public Tag(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
