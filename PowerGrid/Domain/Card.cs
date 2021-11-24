using Newtonsoft.Json;

namespace PowerGrid.Domain
{
    public class Card
    {
        [JsonProperty(PropertyName = "minimumBid")]
        public int MinimumBid { get; set; }

        [JsonProperty(PropertyName = "resource")]
        public ResourceType Resource { get; set; }

        [JsonProperty(PropertyName = "resourceCost")]
        public int ResourceCost { get; set; }

        [JsonProperty(PropertyName = "generatorsPowered")]
        public int GeneratorsPowered { get; set; }
    }
}
