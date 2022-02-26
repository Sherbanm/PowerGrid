﻿
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
        public void TestFullGame()
        {
            Game.Start();
            var gameState = Game.gameState;
            var cards = MockGameState.Cards.ToList();

            var red = gameState.Players.ToList()[0];
            var blue = gameState.Players.ToList()[1];
            var green = gameState.Players.ToList()[2];
            var yellow = gameState.Players.ToList()[3];
            var purple = gameState.Players.ToList()[4];
            var white = gameState.Players.ToList()[5];

            Setup(gameState, cards, red, blue, green, yellow, purple, white);
            Round1(gameState, cards, red, blue, green, yellow, purple, white);
            Round2(gameState, cards, red, blue, green, yellow, purple, white);
            Round3(gameState, cards, red, blue, green, yellow, purple, white);
            Round4(gameState, cards, red, blue, green, yellow, purple, white);
            Round5(gameState, cards, red, blue, green, yellow, purple, white);
            Round6(gameState, cards, red, blue, green, yellow, purple, white);
            Round7(gameState, cards, red, blue, green, yellow, purple, white);

            Game.ExportGameStateToJsonFile("D:\\Workspace\\PowerGrid\\PowerGrid.Tests\\data\\endofround1.json");
            
        }
    
        private void Setup(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white)
        {
            gameState.Players.ToList()[0].Name = "Red";
            gameState.Players.ToList()[1].Name = "Blue";
            gameState.Players.ToList()[2].Name = "Green";
            gameState.Players.ToList()[3].Name = "Yellow";
            gameState.Players.ToList()[4].Name = "Purple";
            gameState.Players.ToList()[5].Name = "White";
            
            gameState.AuctionHouse.Marketplace.Clear();
            gameState.AuctionHouse.DrawPile.Clear();
            
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
        }
    
        private void Round1(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white)
        {
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
            Game.LoadResource(red, red.Cards[0], ResourceType.Gas);
            Game.LoadResource(red, red.Cards[0], ResourceType.Gas);
            Game.AdvanceGame();
            Game.BuyResource(blue, ResourceType.Oil, 1);
            Game.LoadResource(blue, blue.Cards[0], ResourceType.Oil);
            Game.AdvanceGame();
            Game.BuyResource(yellow, ResourceType.Coal, 3);
            Game.LoadResource(yellow, yellow.Cards[0], ResourceType.Coal);
            Game.LoadResource(yellow, yellow.Cards[0], ResourceType.Coal);
            Game.LoadResource(yellow, yellow.Cards[0], ResourceType.Coal);
            Game.AdvanceGame();
            Game.BuyResource(white, ResourceType.Coal, 3);
            Game.LoadResource(white, white.Cards[0], ResourceType.Coal);
            Game.LoadResource(white, white.Cards[0], ResourceType.Coal);
            Game.AdvanceGame();
            Game.BuyResource(purple, ResourceType.Gas, 1);
            Game.LoadResource(purple, purple.Cards[0], ResourceType.Gas);
            Game.AdvanceGame();
            Game.BuyResource(green, ResourceType.Coal, 1);
            Game.LoadResource(green, green.Cards[0], ResourceType.Coal);
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

            // bureaucracy
            Game.AdvanceGame();
        }

        private void Round2(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) 
        {
            // determine player order

            Game.AdvanceGame();
            
            // cards

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 7), green);
            Game.AuctionPassCard(yellow);
            Game.AuctionPassCard(white);
            Game.AuctionPassCard(blue);
            Game.AuctionPassCard(red);
            Game.AuctionPassCard(purple);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 11), purple);
            Game.AuctionPassCard(yellow);
            Game.AuctionPassCard(white);
            Game.AuctionBid(blue, 12);
            Game.AuctionPassCard(red);
            Game.AuctionBid(purple, 13);
            Game.AuctionPassCard(blue);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 31), white);
            Game.AuctionPassCard(blue);
            Game.AuctionPassCard(red);
            Game.AuctionPassCard(yellow);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 32), yellow);
            Game.AuctionPassCard(blue);
            Game.AuctionPassCard(red);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 16), blue);
            Game.AuctionPassCard(red);

            Game.AuctionSetCard(cards.First(x => x.MinimumBid == 23), red);

            // resources

            Game.BuyResource(red, ResourceType.Oil, 2);
            Game.AdvanceGame();
            Game.BuyResource(blue, ResourceType.Gas, 2);
            Game.AdvanceGame();
            Game.BuyResource(yellow, ResourceType.Coal, 3);
            Game.AdvanceGame();
            // white pass
            Game.AdvanceGame();
            Game.BuyResource(purple, ResourceType.Gas, 1);
            Game.AdvanceGame();
            Game.BuyResource(green, ResourceType.Coal, 2);
            Game.AdvanceGame();


            // cities

            var cities = gameState.Map.Cities;
            Game.BuyGenerator(red, cities.First(x => x.Name.Equals("Chicago")));
            Game.AdvanceGame();
            Game.BuyGenerator(blue, cities.First(x => x.Name.Equals("Los_Angeles")));
            Game.BuyGenerator(blue, cities.First(x => x.Name.Equals("Las_Vegas")));
            Game.AdvanceGame();
            // yellow pass
            Game.AdvanceGame();
            // white pass
            Game.AdvanceGame();
            Game.BuyGenerator(purple, cities.First(x => x.Name.Equals("Kansas_City")));
            Game.AdvanceGame();
            Game.BuyGenerator(green, cities.First(x => x.Name.Equals("Seattle")));
            Game.AdvanceGame();

            Game.AdvanceGame();


        }
        private void Round3(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) { }
        private void Round4(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) { }
        private void Round5(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) { }
        private void Round6(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) { }
        private void Round7(GameState gameState, List<Card> cards, Player red, Player blue, Player green, Player yellow, Player purple, Player white) { }
    }
}
