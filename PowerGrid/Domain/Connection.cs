using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Connection
    {
        [JsonProperty(PropertyName = "cityB")]
        public City CityA { get; set; }

        [JsonProperty(PropertyName = "cityA")]
        public City CityB { get; set; }
    }
}
