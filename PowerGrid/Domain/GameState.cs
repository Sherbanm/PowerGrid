﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace PowerGrid.Domain
{
    public class GameState
    {
        [JsonProperty(PropertyName = "players")]
        public LinkedList<Player> Players { get; set; }

        [JsonProperty(PropertyName = "resourceMarket")]
        public ResourceMarket ResourceMarket { get; set; }

        [JsonProperty(PropertyName = "auctionHouse")]
        public AuctionHouse AuctionHouse { get; set; }

        [JsonProperty(PropertyName = "map")]
        public Map Map {get; set; }

        [JsonProperty(PropertyName = "currentPlayer")]
        public Player CurrentPlayer { get; set; }

        [JsonProperty(PropertyName = "currentBidder")]
        public Player CurrentBidder { get; set; }

        [JsonProperty(PropertyName = "currentPhase")]
        public Phase CurrentPhase { get; set; }

        [JsonProperty(PropertyName = "currentStep")]
        public Step CurrentStep  { get; set; }

        [JsonProperty(PropertyName = "remainingTime")]
        public int RemainingTime{ get; set; }

        [JsonProperty(PropertyName = "playerOrder")]
        public LinkedList<Player> PlayerOrder { get; set; } = new LinkedList<Player>();

    }
}
