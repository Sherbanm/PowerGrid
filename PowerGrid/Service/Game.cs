using Microsoft.AspNetCore.Http;
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
                gameState.CurrentPhase = Phase.AuctionPowerPlants;
            }
            else if (gameState.CurrentPhase == Phase.AuctionPowerPlants)
            {
                gameState.CurrentPhase = Phase.BuyResources;
            }
            else if (gameState.CurrentPhase == Phase.BuyResources)
            {
                gameState.CurrentPhase = Phase.Bureaucracy;
            }
            else if (gameState.CurrentPhase == Phase.Bureaucracy)
            {
                gameState.CurrentPhase = Phase.DeterminePlayerOrder;
            }
            
            SendUpdates();
        }

        public static void BuyResource(Player buyer, ResourceType type, int count)
        {
            Player player = gameState.Players[GetPlayerIndex(buyer)];

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
            Player player = gameState.Players[GetPlayerIndex(buyer)];
            int selectedCardIndex = gameState.AuctionHouse.AvailableCards.FindIndex((item) =>
                {
                    if (item.ResourceCost == card.ResourceCost && item.GeneratorsPowered == card.GeneratorsPowered && item.Resource == card.Resource && item.MinimumBid == card.MinimumBid)
                    {
                        return true;
                    }
                    return false;
                });
            
            player.Cards.Add(gameState.AuctionHouse.AvailableCards[selectedCardIndex]);
            gameState.AuctionHouse.AvailableCards.RemoveAt(selectedCardIndex);

            SendUpdates();
        }

        public static void BuyGenerator(Player buyer, City city)
        {
            Player player = gameState.Players[GetPlayerIndex(buyer)];
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


    }
}
