using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PowerGrid.Domain
{
    public class Player : IEquatable<Player>
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "cards")]
        public List<Card> Cards { get; set; } = new List<Card>(3);

        [JsonProperty(PropertyName = "money")]
        public int Money { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public Resources Resources { get; set; } = new Resources();

        [JsonProperty(PropertyName = "generators")]
        public int Generators { get; set; } = 22;

        public bool Equals(Player other)
        {
            return Name.Equals(other.Name);
        }
    }
}
