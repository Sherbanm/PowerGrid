using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PowerGrid.Domain
{
    public class Card : IEquatable<Card>
    {
        [JsonProperty(PropertyName = "minimumBid")]
        public int MinimumBid { get; set; }

        [JsonProperty(PropertyName = "resource")]
        public ResourceType Resource { get; set; }

        [JsonProperty(PropertyName = "resourceCost")]
        public int ResourceCost { get; set; }

        [JsonProperty(PropertyName = "generatorsPowered")]
        public int GeneratorsPowered { get; set; }

        [JsonProperty(PropertyName = "loadedResources")]
        public List<ResourceType> LoadedResources { get; set; } = new List<ResourceType>();
        public bool Equals(Card other)
        {
            return MinimumBid == other.MinimumBid;
        }

        public void LoadResource(ResourceType resource)
        {
            if (Resource == ResourceType.Mixed)
            {
                if (resource == ResourceType.Gas || resource == ResourceType.Oil)
                {
                    LoadedResources.Add(resource);
                }
            }
            else
            {
                if (resource == Resource)
                {
                    LoadedResources.Add(resource);
                }
            }
        }
    }
}
