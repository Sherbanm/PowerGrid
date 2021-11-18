using Newtonsoft.Json;
using System.Collections.Generic;

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

        [JsonProperty(PropertyName = "currentPhase")]
        public Phase CurrentPhase { get; set; }

        [JsonProperty(PropertyName = "currentStep")]
        public Step CurrentStep  { get; set; }

        [JsonProperty(PropertyName = "remainingSeconds")]
        public int RemainingSeconds { get; set; }

        [JsonProperty(PropertyName = "playerOrder")]
        public LinkedList<Player> PlayerOrder { get; set; } = new LinkedList<Player>();
    }
}
