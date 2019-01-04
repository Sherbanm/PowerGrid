using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class AuctionHouse
    {
        [JsonProperty(PropertyName = "availableCards")]
        public Card[] AvailableCards { get; set; }

        [JsonProperty(PropertyName = "displayedCards")]
        public Card[] DisplayedCards { get; set; }

    }
}
