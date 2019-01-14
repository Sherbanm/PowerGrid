using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Domain
{
    public class GameState
    {
        [JsonProperty(PropertyName = "players")]
        public Player[] Players { get; set; }

        [JsonProperty(PropertyName = "resourceMarket")]
        public ResourceMarket ResourceMarket { get; set; }

        [JsonProperty(PropertyName = "auctionHouse")]
        public AuctionHouse AuctionHouse { get; set; }

        [JsonProperty(PropertyName = "map")]
        public Map Map {get; set; }

        [JsonProperty(PropertyName = "currentPlayer")]
        public Player CurrentPlayer { get; set; }

        [JsonProperty(PropertyName = "remainingSeconds")]
        public int RemainingSeconds { get; set; }
    }
}
