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
                // everything costs 2 for now.
                if (player.Money >= 2)
                {
                    player.Money -= 2;
                    player.Resources.Data[(int)type]++;
                    gameState.ResourceMarket.Data[(int)type]--;
                }
            }

            SendUpdates();
        }

        public static void BuyCard(Player buyer, Card card)
        {

        }

        public static void BuyGenerator(Player buyer, City city)
        {

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
