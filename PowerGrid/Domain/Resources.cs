using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Resources
    {
        [JsonProperty(PropertyName = "oil")]
        public int Oil { get; set; }

        [JsonProperty(PropertyName = "gas")]
        public int Gas { get; set; }

        [JsonProperty(PropertyName = "coal")]
        public int Coal { get; set; }

        [JsonProperty(PropertyName = "nuclear")]
        public int Nuclear { get; set; }

    }
}
