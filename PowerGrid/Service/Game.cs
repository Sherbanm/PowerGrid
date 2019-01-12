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
        public static List<WebSocket> listeners = new List<WebSocket>();
        public static WebSocketReceiveResult result;
        private static GameState gameState = MockGameState.GetMockState();

        public static async void AddListener(WebSocket listener)
        {
            listeners.Add(listener);
        }

        public static async void SaveResult(WebSocketReceiveResult result)
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
                    if (item.Cost == card.Cost && item.Power == card.Power && item.Resource == card.Resource && item.Value == card.Value)
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
    }
}
