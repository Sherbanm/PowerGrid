using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class City
    {
        [JsonProperty(PropertyName = "generators")]
        public List<Player> Generators { get; set; } = new List<Player>(3);

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
