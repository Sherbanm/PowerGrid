using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerGrid.Domain
{
    public class AuctionHouse
    {
        [JsonProperty(PropertyName = "drawpile")]
        public List<Card> DrawPile { get; set; } = new List<Card>();

        [JsonProperty(PropertyName = "marketplace")]
        public List<Card> Marketplace { get; set; }

        [JsonProperty(PropertyName = "playerswhopassedauction")]
        public List<Player> PlayersWhoPassedAuction { get; set; } = new List<Player>();

        [JsonProperty(PropertyName = "playerswhopassedphase")]
        public List<Player> PlayersWhoPassedPhase { get; set; } = new List<Player>();

        [JsonProperty(PropertyName = "playerwhobought")]
        public List<Player> PlayersWhoBought { get; set; } = new List<Player>();

        [JsonProperty(PropertyName = "currentbid")]
        public int CurrentBid { get; set; } = 0;

        [JsonProperty(PropertyName = "currentbidplayer")]
        public Player CurrentBidPlayer { get; set; }

        [JsonProperty(PropertyName = "currentauctioncard")]
        public Card CurrentAuctionCard { get; set; }



        private int marketSize;
        
        private static Random random = new Random();

        

        public void Prepare(IEnumerable<Card> darkBackSideCards, IEnumerable<Card> lightBackSideCards, bool northAmerica, int numberOfPlayers)
        {
            // northamerica
            Card faceDownTemporary = null;
            
            // in north america the market consists of 8 spaces, in Europ, the market has 9 spaces.
            this.marketSize = northAmerica ? 8 : 9;

            // Take the power plant cards with a dark backside (numbered 03 to 15) and shuffle this pile
            var shuffledDarkBackSideCards = darkBackSideCards.OrderBy(x => random.Next()).ToList();

            // draw the top X cards and place them face up. Sort them in ascending order by their number, place those with the smallest number in the top row of the market in ascending order , left to right
            var initialMarketCards = shuffledDarkBackSideCards.Take(marketSize).OrderBy(x => x.MinimumBid).ToList();
            shuffledDarkBackSideCards = shuffledDarkBackSideCards.Skip(marketSize).ToList();
            if (northAmerica)
            {
                faceDownTemporary = shuffledDarkBackSideCards.First();
                shuffledDarkBackSideCards = shuffledDarkBackSideCards.Skip(1).ToList();
            }

            // shuffle the power plant cards with a light backside
            var shuffledLightBackSideCards = lightBackSideCards.OrderBy(x => random.Next()).ToList();
            // remove some power plant cards as shown in the table
            shuffledLightBackSideCards = shuffledLightBackSideCards.Skip(GetPowerPlantDiscards(true, numberOfPlayers)).ToList();
            shuffledDarkBackSideCards = shuffledDarkBackSideCards.Skip(GetPowerPlantDiscards(false, numberOfPlayers)).ToList();

            var shuffledAllCards = shuffledDarkBackSideCards.Union(shuffledLightBackSideCards).OrderBy(x => random.Next()).ToList();
            if (northAmerica)
            {
                shuffledAllCards.Insert(0, faceDownTemporary);
            }
            Marketplace = initialMarketCards;
            DrawPile = shuffledAllCards;
        }

        private int GetPowerPlantDiscards(bool light, int numberOfPlayers)
        {
            if (light)
            {
                if (numberOfPlayers == 2)
                    return 1;
                else if (numberOfPlayers == 3)
                    return 2;
                else if (numberOfPlayers == 4)
                    return 1;
                else if (numberOfPlayers >= 5)
                    return 0;
            }
            else
            {
                if (numberOfPlayers == 2)
                    return 5;
                else if (numberOfPlayers == 3)
                    return 6;
                else if (numberOfPlayers == 4)
                    return 3;
                else if (numberOfPlayers >= 5)
                    return 0;
            }
            throw new Exception();
        }

        public void SetCard(Card card, Player player)
        {
            CurrentAuctionCard = card;
            CurrentBid = card.MinimumBid;
            CurrentBidPlayer = player;
        }

        public void Bid(Player player, int bid)
        {
            CurrentBid = bid;
            CurrentBidPlayer = player;
        }

        public void PassCard(Player player)
        {
            if (!PlayersWhoPassedAuction.Contains(player))
            {
                PlayersWhoPassedAuction.Add(player);
            }
        }

        public void Cleanup()
        {
            var winner = CurrentBidPlayer;
            PlayersWhoPassedAuction.Clear();
            PlayersWhoBought.Add(winner);
            CurrentBid = 0;
            CurrentBidPlayer = null;
            CurrentAuctionCard = null;

            Draw();
        }

        public void RemoveFirst()
        {
            Marketplace.RemoveAt(0);
            Draw();
        }

        private void Draw()
        {
            var newCard = DrawPile.First();
            Marketplace.Add(newCard);
            Marketplace = Marketplace.OrderBy(x => x.MinimumBid).ToList();
            DrawPile.Remove(newCard);
        }
    }
}
