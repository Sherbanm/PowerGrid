using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid.Service
{
    public static class Game
    {
        private static GameState gameState = MockGameState.GetMockState();

        public static void BuyResource(Player buyer, ResourceType type, int count)
        {
            Player player = gameState.Players[GetPlayerIndex(buyer)];

            for(int i = 0; i < count; i++)
            {
                // everything costs 2 for now.
                if (player.Money >= 2)
                {
                    player.Money-= 2;
                    player.Resources.Oil++;
                    gameState.ResourceMarket.Oil--;
                }
            }
        }

        public static void BuyCard(Player buyer, Card card)
        {

        }

        public static void BuyGenerator(Player buyer, City city)
        {

        }

        private static int GetPlayerIndex(Player player) 
        {
            for(int i = 0; i < gameState.Players.Length; i++)
            {
                if (gameState.Players[i].Name == player.Name)
                {
                    return i;
                }
            }
            return -1;
        }

        private static int GetCardIndex(Card card)
        {
            for (int i = 0; i < gameState.AuctionHouse.AvailableCards.Length; i++)
            {
                if (gameState.AuctionHouse.AvailableCards[i].Cost == card.Cost &&
                    gameState.AuctionHouse.AvailableCards[i].Power == card.Power &&
                    gameState.AuctionHouse.AvailableCards[i].Resource == card.Resource &&
                    gameState.AuctionHouse.AvailableCards[i].Value == card.Value)
                {
                    return i;
                }
            }
            return -1;
        }

        private static int GetCityIndex(City city)
        {
            for (int i = 0; i < gameState.Map.Cities.Length; i++)
            {
                if (gameState.Map.Cities[i].Name == city.Name)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
