using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Resources
    {
        [JsonProperty(PropertyName = "data")]
        public int[] Data { get; set; } = new int[4];
              
    }
}
