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
        public List<Card> AvailableCards { get; set; } = new List<Card>();

        [JsonProperty(PropertyName = "displayedCards")]
        public List<Card> DisplayedCards { get; set; } = new List<Card>();

    }
}
