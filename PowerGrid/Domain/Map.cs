using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Map
    {
        [JsonProperty(PropertyName = "connections")]
        public Connection[] Connections{ get; set; }

        [JsonProperty(PropertyName = "cities")]
        public List<City> Cities { get; set; }
    }
}
