
using PowerGrid.Domain;
using PowerGrid.Service;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerGrid.Tests.BackEndLogic
{
    public class GameStateTests
    {
        [Fact]
        public void Round1()
        {
            Game.Start();
            var gameState = Game.gameState;
            gameState.Players.ToList()[0].Name = "Red";
            gameState.Players.ToList()[1].Name = "Blue";
            gameState.Players.ToList()[2].Name = "Green";
            gameState.Players.ToList()[3].Name = "Yellow";
            gameState.Players.ToList()[4].Name = "Purple";
            gameState.Players.ToList()[5].Name = "White";
            var red = gameState.Players.ToList()[0];
            var blue = gameState.Players.ToList()[1];
            var green = gameState.Players.ToList()[2];
            var yellow = gameState.Players.ToList()[3];
            var purple = gameState.Players.ToList()[4];
            var white = gameState.Players.ToList()[5];
            gameState.AuctionHouse.Marketplace.Clear();
            gameState.AuctionHouse.DrawPile.Clear();
            var cards = MockGameState.Cards.ToList();

            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 5));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 6));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 7));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 9));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 12));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 13));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 14));
            gameState.AuctionHouse.Marketplace.Add(cards.First(x => x.MinimumBid == 15));

            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 11));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 31));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 39));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 34));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 32));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 40));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 23));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 42));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 16));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 35));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 36));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 46));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 25));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 37));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 30));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 22));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 10));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 19));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 4));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 3));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 8));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 28));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 38));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 44));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 36));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 24));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 26));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 29));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 50));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 17));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 33));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 27));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 20));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 21));
            gameState.AuctionHouse.DrawPile.Add(cards.First(x => x.MinimumBid == 18));

            Game.AdvanceGame();
            var order = new List<Player>();
            var players = new List<Player>();
            players.Add(blue);
            players.Add(red);
            players.Add(purple);
            players.Add(green);
            players.Add(yellow);
            players.Add(white);

            order.Add(blue);
            order.Add(red);
            order.Add(yellow);
            order.Add(white);
            order.Add(purple);
            order.Add(green);
            gameState.Players = new LinkedList<Player>(players);
            gameState.PlayerOrder = new LinkedList<Player>(order);
            gameState.CurrentPlayer = blue;
            gameState.CurrentBidder = blue;

            // cards

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 6), blue);
            Game.AuctionPassCard(red);
            Game.AuctionPassCard(purple);
            Game.AuctionPassCard(green);
            Game.AuctionPassCard(yellow);
            Game.AuctionPassCard(white);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 5), red);
            Game.AuctionPassCard(purple);
            Game.AuctionPassCard(green);
            Game.AuctionBid(yellow, 6);
            Game.AuctionPassCard(white);
            Game.AuctionBid(red, 7);
            Game.AuctionPassCard(yellow);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 9), yellow);
            Game.AuctionPassCard(white);
            Game.AuctionPassCard(purple);
            Game.AuctionPassCard(green);
            
            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 12), white);
            Game.AuctionPassCard(purple);
            Game.AuctionPassCard(green);
            
            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 14), purple);
            Game.AuctionBid(green, 15);
            Game.AuctionBid(purple, 16);
            Game.AuctionPassCard(green);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 15), green);

            // resources

            Game.BuyResource(red, ResourceType.Gas, 2);
            Game.AdvanceGame();
            Game.BuyResource(blue, ResourceType.Oil, 1);
            Game.AdvanceGame();
            Game.BuyResource(yellow, ResourceType.Coal, 3);
            Game.AdvanceGame();
            Game.BuyResource(white, ResourceType.Coal, 3);
            Game.AdvanceGame();
            Game.BuyResource(purple, ResourceType.Gas, 1);
            Game.AdvanceGame();
            Game.BuyResource(green, ResourceType.Coal, 1);
            Game.AdvanceGame();


            // cities

            var cities = gameState.Map.Cities;
            Game.BuyGenerator(red, cities.First(x => x.Name.Equals("Milwaukee")));
            Game.AdvanceGame();
            Game.BuyGenerator(blue, cities.First(x => x.Name.Equals("San_Diego")));
            Game.AdvanceGame();
            Game.BuyGenerator(yellow, cities.First(x => x.Name.Equals("Mexico_City")));
            Game.AdvanceGame();
            Game.BuyGenerator(white, cities.First(x => x.Name.Equals("Miami")));
            Game.AdvanceGame();
            Game.BuyGenerator(purple, cities.First(x => x.Name.Equals("St_Louis")));
            Game.AdvanceGame();
            Game.BuyGenerator(green, cities.First(x => x.Name.Equals("Vancouver")));
            Game.AdvanceGame();

            Game.AdvanceGame();

            Game.ExportGameStateToJsonFile("D:\\Workspace\\PowerGrid\\PowerGrid.Tests\\data\\endofround1.json");
            
        }
    }
}
