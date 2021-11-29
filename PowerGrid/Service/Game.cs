﻿using Microsoft.AspNetCore.Http;
using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerGrid.Service
{
    public static class Game
    {
        private static List<WebSocket> listeners = new List<WebSocket>();
        private static WebSocketReceiveResult result;
        private static GameState gameState = MockGameState.GetMockState();

        public static void AddListener(WebSocket listener)
        {
            listeners.Add(listener);
        }

        public static void SaveResult(WebSocketReceiveResult result)
        {
            Game.result = result;
        }

        public static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            AddListener(webSocket);
            SaveResult(result);
            while (!result.CloseStatus.HasValue)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(MockGameState.GetMockState());
                var bytes = Encoding.GetEncoding(Encoding.UTF8.BodyName).GetBytes(json.ToCharArray());

                await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public static void AdvanceGame()
        {
            if (gameState.CurrentPhase == Phase.DeterminePlayerOrder)
            {
                // beginning of the game
                if (gameState.PlayerOrder.Count == 0)
                {
                    var random = new Random();
                    Dictionary<Player, int> playerRolls = new Dictionary<Player, int>();
                    foreach(var player in gameState.Players)
                    {
                        playerRolls[player] = random.Next();
                    }
                    var orderedList = playerRolls.OrderBy(x => x.Value).Select(x => x.Key);
                    gameState.PlayerOrder = new LinkedList<Player>(orderedList);
                }
                else
                {
                    var orderedList = gameState.Players.OrderByDescending(x => CountGenerator(gameState, x)).ThenByDescending(x => GetBiggestPowerPlant(x));
                    gameState.PlayerOrder = new LinkedList<Player>(orderedList);
                }
                gameState.CurrentPlayer = gameState.PlayerOrder.First();
                gameState.CurrentBidder = gameState.PlayerOrder.First();
                gameState.CurrentPhase = Phase.AuctionPowerPlants;
            }
            else if (gameState.CurrentPhase == Phase.AuctionPowerPlants)
            {
                gameState.AuctionHouse.PhasePassers.Add(gameState.CurrentPlayer);
                var curPlayer = gameState.PlayerOrder.Find(gameState.CurrentPlayer);
                var nextPlayer = curPlayer.Next.Value;
                gameState.CurrentPlayer = nextPlayer;
                gameState.CurrentBidder = nextPlayer;

                if (gameState.AuctionHouse.PhaseBuyers.Count + gameState.AuctionHouse.PhasePassers.Count == gameState.Players.Count)
                {
                    gameState.CurrentPhase = Phase.BuyResources;
                    gameState.AuctionHouse.PhasePassers.Clear();
                    gameState.AuctionHouse.PhaseBuyers.Clear();
                }
            }
            else if (gameState.CurrentPhase == Phase.BuyResources)
            {
                if (gameState.CurrentPlayer.Equals(gameState.PlayerOrder.First.Value))
                {
                    gameState.CurrentPlayer = gameState.PlayerOrder.Last.Value;
                    gameState.CurrentPhase = Phase.BuildGenerators;
                }
                else
                {
                    var curPlayer = gameState.PlayerOrder.Find(gameState.CurrentPlayer);
                    var previousPlayer = curPlayer.Previous.Value;
                    gameState.CurrentPlayer = previousPlayer;
                }
            }
            else if (gameState.CurrentPhase == Phase.BuildGenerators)
            {
                if (gameState.CurrentPlayer.Equals(gameState.PlayerOrder.First.Value))
                {
                    gameState.CurrentPhase = Phase.Bureaucracy;
                }
                else
                {
                    var curPlayer = gameState.PlayerOrder.Find(gameState.CurrentPlayer);
                    var previousPlayer = curPlayer.Previous.Value;
                    gameState.CurrentPlayer = previousPlayer;
                }
            }
            else if (gameState.CurrentPhase == Phase.Bureaucracy)
            {
                gameState.CurrentPhase = Phase.DeterminePlayerOrder;
            }
            
            SendUpdates();
        }

        public static void BuyResource(Player buyer, ResourceType type, int count)
        {
            Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));

            for(int i = 0; i < count; i++)
            {
                int cost = gameState.ResourceMarket.GetCost(type);
                if (player.Money >= cost)
                {
                    player.Money -= cost;
                    player.Resources.Data[(int)type]++;
                    gameState.ResourceMarket.Resources.Data[(int)type]--;
                }
            }

            SendUpdates();
        }

        public static void BuyCard(Player buyer, Card card)
        {
            Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));
            int selectedCardIndex = gameState.AuctionHouse.Marketplace.FindIndex((item) =>
                {
                    if (item.ResourceCost == card.ResourceCost && item.GeneratorsPowered == card.GeneratorsPowered && item.Resource == card.Resource && item.MinimumBid == card.MinimumBid)
                    {
                        return true;
                    }
                    return false;
                });
            
            player.Cards.Add(gameState.AuctionHouse.Marketplace[selectedCardIndex]);
            gameState.AuctionHouse.Marketplace.RemoveAt(selectedCardIndex);
        
            SendUpdates();
        }

        public static void SetAuctionedCard(Card card, Player player)
        {
            gameState.AuctionHouse.SetAuctionedCard(card, player);
            SendUpdates();
        }

        public static void Bid(Player player, int amount)
        {
            gameState.AuctionHouse.Bid(player, amount);

            gameState.CurrentBidder = GetNextBidder();

            SendUpdates();
        }

        public static void Pass(Player player)
        {
            gameState.AuctionHouse.Pass(player);

            if (gameState.AuctionHouse.PhaseBuyers.Count + gameState.AuctionHouse.PhasePassers.Count + gameState.AuctionHouse.AuctionPassedPlayers.Count == gameState.Players.Count - 1)
            {
                var winner = gameState.AuctionHouse.CurrentBidPlayer;
                var wonCard = gameState.AuctionHouse.DrawPile.Find(x => gameState.AuctionHouse.CardUnderAuction.Equals(x));
                winner.Cards.Add(wonCard);
                gameState.AuctionHouse.DrawPile.Remove(wonCard);

                // cleanup
                gameState.AuctionHouse.AuctionPassedPlayers.Clear();
                gameState.AuctionHouse.PhaseBuyers.Add(winner);
                gameState.AuctionHouse.CurrentBid = 0;
                gameState.AuctionHouse.CurrentBidPlayer = null;
                gameState.AuctionHouse.CardUnderAuction = null;

            }
            else
            {
                gameState.CurrentBidder = GetNextBidder();
            }
            

            SendUpdates();
        }

        public static void BuyGenerator(Player buyer, City city)
        {
            Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));
            int selectedCityIndex = gameState.Map.Cities.FindIndex((item) =>
            {
                return (item.Name == city.Name);
            });

            player.Generators--;
            gameState.Map.Cities[selectedCityIndex].Generators.Add(player);

            SendUpdates();
        }

        public static void SendUpdates()
        {
            foreach (WebSocket listener in listeners)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(gameState);
                var bytes = Encoding.GetEncoding(Encoding.UTF8.BodyName).GetBytes(json.ToCharArray());

                listener.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

            }
        }

        private static int CountGenerator(GameState gameState, Player player)
        {
            var counter = 0;
            foreach (var city in gameState.Map.Cities)
            {
                if (city.Generators.Contains(player))
                {
                    counter++;
                }
            }
            return counter;
        }

        private static int GetBiggestPowerPlant(Player player)
        {
            var highestValue = 0;
            if (player.Cards.Count > 0)
            {
                highestValue = player.Cards.OrderByDescending(x => x.MinimumBid).First().MinimumBid;
            }
            return highestValue;
        }

        private static Player GetNextBidder()
        {
            Player nextPlayer = gameState.Players.Find(gameState.CurrentBidder).Next?.Value ?? gameState.Players.First.Value; ;
            bool found = false;
            for (int i = 0; i < gameState.Players.Count; i++)
            {
                if (!gameState.AuctionHouse.AuctionPassedPlayers.Contains(nextPlayer) && !gameState.AuctionHouse.PhaseBuyers.Contains(nextPlayer) && !gameState.AuctionHouse.PhasePassers.Contains(nextPlayer))
                {
                    found = true;
                    break;
                }
                else
                {
                    nextPlayer = gameState.Players.Find(nextPlayer).Next?.Value ?? gameState.Players.First.Value;
                }
            }
            return found ? nextPlayer : null;
        }


    }
}
