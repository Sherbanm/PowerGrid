using Newtonsoft.Json;
using System.Collections.Generic;

namespace PowerGrid.Domain
{
    public class City
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }


        [JsonProperty(PropertyName = "originid")]
        public string OrigID { get; set; }

        [JsonProperty(PropertyName = "generators")]
        public List<Player> Generators { get; set; } = new List<Player>(3);

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "mass")]
        public float Mass { get; set; } = 1.0f;

        [JsonProperty(PropertyName = "pinned")]
        public bool Pinned { get; set; }

        [JsonProperty(PropertyName = "build")]
        public bool Build { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Cost { get; set; }


    }
}
