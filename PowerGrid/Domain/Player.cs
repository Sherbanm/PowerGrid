﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class Player
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "cards")]
        public Card[] Cards { get; set; }

        [JsonProperty(PropertyName = "money")]
        public int Money { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public Resources Resources{ get; set; }
    }
}
