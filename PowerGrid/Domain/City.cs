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
        public Player[] Generators{ get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
