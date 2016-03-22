﻿using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class Tip
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; }

        public Tip() { }

        public Tip(string id, string prompt)
        {
            this.ID = id;
            this.Prompt = prompt;
        }
    }
}