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
                gameState.CurrentBidder = gameState.PlayerOrder.First();
                gameState.CurrentPhase = Phase.AuctionPowerPlants;
            }
            else if (gameState.CurrentPhase == Phase.AuctionPowerPlants)
            {
                if (gameState.AuctionHouse.PhaseBuyers.Count + gameState.AuctionHouse.PhasePassers.Count == gameState.Players.Count)
                {
                    gameState.CurrentPhase = Phase.BuyResources;
                    gameState.CurrentBidder = null;
                    gameState.AuctionHouse.PhasePassers.Clear();
                    gameState.AuctionHouse.PhaseBuyers.Clear();
                    return;
                }
                var curPlayer = gameState.PlayerOrder.Find(gameState.CurrentPlayer);
                var nextPlayer = GetNextPlayer();
                gameState.CurrentPlayer = nextPlayer;
                gameState.CurrentBidder = nextPlayer;
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
                foreach(var player in gameState.Players)
                {
                    var loadedCards = player.Cards.Where(x => x.LoadedResources.Count.Equals(x.ResourceCost));
                    var canPower = loadedCards.Sum(x => x.GeneratorsPowered);
                    var generators = gameState.Map.Cities.Where(x => x.Generators.Contains(player)).Count();
                    var generatorsPowered = Math.Min(canPower, generators);
                    var income = GetIncome(generatorsPowered);

                    player.Money += income;
                    foreach(var card in loadedCards)
                    {
                        player.Cards.Find(x => x.Equals(card)).LoadedResources.Clear();
                    }
                }
                gameState.CurrentPhase = Phase.DeterminePlayerOrder;
            }
            
            SendUpdates();
        }

        public static void AuctionSetCard(Card card, Player player)
        {
            gameState.AuctionHouse.SetCard(card, player);
            gameState.AuctionHouse.Bid(player, card.MinimumBid);
            gameState.CurrentBidder = GetNextBidder();
            if (gameState.AuctionHouse.PhaseBuyers.Count + gameState.AuctionHouse.PhasePassers.Count + gameState.AuctionHouse.AuctionPassedPlayers.Count == gameState.Players.Count - 1)
            {
                BuyCard();
                Cleanup();
                AdvanceGame();
            }
            SendUpdates();
        }

        public static void AuctionBid(Player player, int amount)
        {
            gameState.AuctionHouse.Bid(player, amount);

            gameState.CurrentBidder = GetNextBidder();

            SendUpdates();
        }

        public static void AuctionPassCard(Player player)
        {
            gameState.AuctionHouse.PassCard(player);

            if (gameState.AuctionHouse.PhaseBuyers.Count + gameState.AuctionHouse.PhasePassers.Count + gameState.AuctionHouse.AuctionPassedPlayers.Count == gameState.Players.Count - 1)
            {
                bool currentPlayerWon = gameState.AuctionHouse.CurrentBidPlayer.Equals(gameState.CurrentPlayer);
                BuyCard();
                Cleanup();
                if (currentPlayerWon)
                {
                    AdvanceGame();
                }
            }
            else
            {
                gameState.CurrentBidder = GetNextBidder();
            }
            SendUpdates();
        }

        public static void AuctionPassPhase(Player player)
        {
            gameState.AuctionHouse.PhasePassers.Add(player);
            AdvanceGame();
        }

        public static void BuyResource(Player buyer, ResourceType type, int count)
        {
            Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));

            for (int i = 0; i < count; i++)
            {
                int cost = gameState.ResourceMarket.GetCost(type);
                if (player.Money >= cost)
                {
                    player.Money -= cost;
                    player.Resources.Data[(int)type]++;
                    gameState.ResourceMarket.Resources.Data[(int)type]--;
                }
            }


            throw new Exception("abracadabra");
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
        
        public static void LoadResource(Card card, ResourceType resource)
        {
            var gameStateCard = gameState.Players.SelectMany(x => x.Cards).First(x => x.Equals(card));
            var gameStatePlayer = gameState.Players.First(x => x.Cards.Contains(card));

            if (gameStatePlayer.Resources.Data[(int)resource] > 0)
            {
                gameStatePlayer.Resources.Data[(int)resource]--;
                gameStateCard.LoadResource(resource);
            }
            SendUpdates();
        }

        private static void SendUpdates()
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
            Player nextBider = gameState.Players.Find(gameState.CurrentBidder).Next?.Value ?? gameState.Players.First.Value;
            bool found = false;
            for (int i = 0; i < gameState.Players.Count; i++)
            {
                if (!gameState.AuctionHouse.AuctionPassedPlayers.Contains(nextBider) && !gameState.AuctionHouse.PhaseBuyers.Contains(nextBider) && !gameState.AuctionHouse.PhasePassers.Contains(nextBider))
                {
                    found = true;
                    break;
                }
                else
                {
                    nextBider = gameState.Players.Find(nextBider).Next?.Value ?? gameState.Players.First.Value;
                }
            }
            return found ? nextBider : null;
        }

        private static Player GetNextPlayer()
        {
            Player nextPlayer = gameState.PlayerOrder.Find(gameState.CurrentPlayer).Next?.Value;
            bool found = false;
            for (int i = 0; i < gameState.PlayerOrder.Count; i++)
            {
                if (!gameState.AuctionHouse.PhaseBuyers.Contains(nextPlayer) && !gameState.AuctionHouse.PhasePassers.Contains(nextPlayer))
                {
                    found = true;
                    break;
                }
                else
                {
                    nextPlayer = gameState.PlayerOrder.Find(nextPlayer).Next?.Value;
                }
            }
            return found ? nextPlayer : null;
        }
    
        private static void BuyCard()
        {
            var winner = gameState.Players.First(x => x.Equals(gameState.AuctionHouse.CurrentBidPlayer));
            var wonCard = gameState.AuctionHouse.Marketplace.Find(x => gameState.AuctionHouse.CardUnderAuction.Equals(x));
            winner.Cards.Add(wonCard);
            winner.Money -= gameState.AuctionHouse.CurrentBid;
            gameState.AuctionHouse.Marketplace.Remove(wonCard);
        }

        private static void Cleanup()
        {
            var winner = gameState.AuctionHouse.CurrentBidPlayer;
            gameState.AuctionHouse.AuctionPassedPlayers.Clear();
            gameState.AuctionHouse.PhaseBuyers.Add(winner);
            gameState.AuctionHouse.CurrentBid = 0;
            gameState.AuctionHouse.CurrentBidPlayer = null;
            gameState.AuctionHouse.CardUnderAuction = null;
        }

        private static int GetIncome(int poweredGenerators)
        {
            switch(poweredGenerators)
            {
                case 0:
                    return 10;
                case 1:
                    return 22;
                case 2:
                    return 33;
                case 3:
                    return 44;
                case 4:
                    return 54;
                case 5:
                    return 64;
                case 6:
                    return 73;
                case 7:
                    return 82;
                case 8:
                    return 90;
                case 9:
                    return 98;
                case 10:
                    return 105;
                case 11:
                    return 112;
                case 12:
                    return 118;
                case 13:
                    return 124;
                case 14:
                    return 129;
                case 15:
                    return 134;
                case 16:
                    return 138;
                case 17:
                    return 142;
                case 18:
                    return 145;
                case 19:
                    return 148;
                case 20:
                    return 150;
                default:
                    return 150;
                
            }
        }
    }
}
