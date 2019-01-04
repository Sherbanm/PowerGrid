using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Card
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "resource")]
        public ResourceType Resource { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "power")]
        public int Power { get; set; }
    }
}
