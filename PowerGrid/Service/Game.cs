using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerGrid.Service
{
    public static class Game
    {
        public static GameState gameState;

        private static List<WebSocket> listeners = new List<WebSocket>();
        private static WebSocketReceiveResult result;
        private static bool firstAuctionPhaseCompleted = false;
        private static Timer timer;

        private static LinkedList<Player> CreatePlayers(IEnumerable<string> playerNames)
        {
            List<Player> list = playerNames.Select(x => new Player { Name = x, Money = 50 }).ToList();
            return new LinkedList<Player>(list);
        }

        private static ResourceMarket CreateResourceMarket() {
            return new ResourceMarket
            {
                Resources = new Resources
                {
                    AvailableResources = new int[] { 23, 18, 14, 1 }
                }
            };
        }

        private static AuctionHouse CreateAuctionHouse(List<Card> cards, IEnumerable<string> playerNames)
        {
            var auctionHouse = new AuctionHouse();
            auctionHouse.Prepare(cards.Where(x => x.MinimumBid <= 15), cards.Where(x => x.MinimumBid > 15), true, playerNames.Count());
            return auctionHouse;
        }

        public static void Start(IEnumerable<string> playerNames)
        {
            gameState = new GameState
            {
                Players = CreatePlayers(playerNames),
                ResourceMarket = CreateResourceMarket(),
                AuctionHouse = CreateAuctionHouse(MockGameState.Cards, playerNames),
                Map = MockGameState.Map,
                RemainingTime = 10
            };
        }

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
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (message.Equals("start"))
                {
                    Start(new List<string> { "Serban", "CPU"} );
                }
                AdvanceGame();
                var json = JsonConvert.SerializeObject(gameState);
                var bytes = Encoding.GetEncoding(Encoding.UTF8.BodyName).GetBytes(json.ToCharArray());

                await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public static void AdvanceGame(bool fromClient = false)
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
                if (!fromClient)
                {
                    // check done auctionining
                    if (gameState.AuctionHouse.PlayersWhoBought.Count + gameState.AuctionHouse.PlayersWhoPassedPhase.Count == gameState.Players.Count)
                    {
                        // reorder first time
                        if (!firstAuctionPhaseCompleted)
                        {
                            var orderedList = gameState.Players.OrderByDescending(x => CountGenerator(gameState, x)).ThenByDescending(x => GetBiggestPowerPlant(x));
                            gameState.PlayerOrder = new LinkedList<Player>(orderedList);
                            gameState.CurrentPlayer = gameState.PlayerOrder.Last();
                            firstAuctionPhaseCompleted = true;
                        }
                        // TODO: remove the discounted card if still there
                        gameState.CurrentPhase = Phase.BuyResources;
                        gameState.AuctionHouse.PlayersWhoPassedPhase.Clear();
                        gameState.AuctionHouse.PlayersWhoBought.Clear();
                        gameState.CurrentBidder = null;
                        gameState.CurrentPlayer = gameState.PlayerOrder.Last();
                        return;
                    }
                    var nextPlayer = GetNextPlayer();
                    gameState.CurrentPlayer = nextPlayer;
                    gameState.CurrentBidder = nextPlayer;
                }
                else
                {
                    throw new Exception("now allowed!");
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
                // check step 2
                var biggestGrid = gameState.Map.Grids.Select(x => x.Value.Count()).Max();
                if ((gameState.Players.Count() == 6 && biggestGrid >= 6) || biggestGrid >= 7)
                {
                    if (gameState.CurrentStep == Step.Step1)
                    {
                        gameState.CurrentStep = Step.Step2;

                        // north america only
                        gameState.AuctionHouse.RemoveIndex(0);
                        gameState.AuctionHouse.Draw(gameState.CurrentStep);
                        CheckAuctionHouse();
                    }
                }

                // earn cash
                foreach (var player in gameState.Players)
                {
                    var loadedCards = player.Cards.Where(x => x.LoadedResources.Count.Equals(x.ResourceCost));
                    var canPower = loadedCards.Sum(x => x.GeneratorsPowered);
                    var generators = gameState.Map.Cities.Where(x => x.Generators.Contains(player)).Count();
                    var generatorsPowered = Math.Min(canPower, generators);
                    var income = GetIncome(generatorsPowered);

                    player.Money += income;
                    // consume
                    var greenPower = loadedCards.Where(x => x.Resource == ResourceType.Green).Sum(x => x.GeneratorsPowered);
                    var dirtyPowerNeeded = generatorsPowered - greenPower;
                    if (dirtyPowerNeeded > 0)
                    {
                        foreach (var card in loadedCards.Where(x => x.Resource != ResourceType.Green).OrderByDescending(x => x.GeneratorsPowered))
                        {
                            player.Cards.Find(x => x.Equals(card)).LoadedResources.Clear();
                            dirtyPowerNeeded -= card.GeneratorsPowered;
                            if (dirtyPowerNeeded <= 0)
                                break;
                        }
                    }
                }
                // resupply the resource market
                gameState.ResourceMarket.Resupply(gameState.Players.ToList(), gameState.CurrentStep, true);

                // update the power plant market
                if (gameState.CurrentStep == Step.Step1 || gameState.CurrentStep == Step.Step2)
                {
                    gameState.AuctionHouse.PlaceHighestBackInDeck();
                    gameState.AuctionHouse.Draw(gameState.CurrentStep);
                    CheckAuctionHouse();
                }
                else
                {
                    gameState.AuctionHouse.RemoveIndex(0);
                    gameState.AuctionHouse.Draw(gameState.CurrentStep);
                    CheckAuctionHouse();
                }
                gameState.CurrentPhase = Phase.DeterminePlayerOrder;
            }

            gameState.RemainingTime = 60;
            SendUpdates();
            Console.WriteLine(DateTime.Now);
            // give them 2 seconds for leeway
            timer = new Timer( (_) => {
                AutoResolve();
                AdvanceGame(false);
            }, null, (gameState.RemainingTime + 2) * 1000, Timeout.Infinite);
        }

        private static void AutoResolve()
        {
            if (gameState.CurrentPhase == Phase.DeterminePlayerOrder)
            {
            }
            else if (gameState.CurrentPhase == Phase.AuctionPowerPlants)
            {
                if (gameState.CurrentBidder == gameState.CurrentPlayer)
                {
                    if (!firstAuctionPhaseCompleted)
                    {
                        var cheapestCard = gameState.AuctionHouse.Marketplace.First();
                        AuctionSetCard(cheapestCard, gameState.CurrentPlayer);
                    }
                    else
                    {
                        AuctionPassPhase(gameState.CurrentPlayer);
                    }
                }
                else
                {
                    AuctionPassCard(gameState.CurrentBidder);
                }
            }
            else if (gameState.CurrentPhase == Phase.BuyResources)
            {
            }
            else if (gameState.CurrentPhase == Phase.BuildGenerators)
            {
            }
            else if (gameState.CurrentPhase == Phase.Bureaucracy)
            {
            }
        }
        public static void AuctionSetCard(Card card, Player player)
        {
            var validPhase = gameState.CurrentPhase == Phase.AuctionPowerPlants;
            if (validPhase)
            {
                var validPlayer = player.Equals(gameState.CurrentPlayer);
                var marketSize = gameState.CurrentStep != Step.Step3 ? 4 : 8;
                var validCard = gameState.AuctionHouse.Marketplace.Take(marketSize).Contains(card);
                if (validPlayer && validCard)
                {
                    gameState.AuctionHouse.SetCard(card, player);
                    gameState.AuctionHouse.Bid(player, card.MinimumBid);
                    gameState.CurrentBidder = GetNextBidder();
                    if (gameState.AuctionHouse.PlayersWhoBought.Count + gameState.AuctionHouse.PlayersWhoPassedPhase.Count + gameState.AuctionHouse.PlayersWhoPassedAuction.Count == gameState.Players.Count - 1)
                    {
                        BuyCard();
                        CleanupAuctionHouse();
                        AdvanceGame();
                    }
                }
                else
                {
                    if (!validPlayer)
                        throw new Exception("invalid player.");
                    else
                        throw new Exception("invalid card.");
                }
            }
            else
            {
                throw new Exception("invalid phase.");
            }
            SendUpdates();
        }

        public static void AuctionBid(Player player, int amount)
        {
            var validPlayer = gameState.CurrentBidder.Equals(player);
            var validAmount = amount > gameState.AuctionHouse.CurrentBid && amount <= player.Money;
            if (validPlayer && validAmount)
            {
                gameState.AuctionHouse.Bid(player, amount);
                gameState.CurrentBidder = GetNextBidder();
            }
            else
            {
                if (!validPlayer)
                    throw new Exception("invalid player.");
                else
                    throw new Exception("invalid amount.");
            }
            SendUpdates();
        }

        public static void AuctionPassCard(Player player)
        {
            var validPhase = gameState.CurrentPhase == Phase.AuctionPowerPlants;
            if (validPhase)
            {
                var validPlayer = gameState.CurrentBidder.Equals(player);
                if (validPlayer)
                {
                    gameState.AuctionHouse.PassCard(player);

                    if (gameState.AuctionHouse.PlayersWhoBought.Count + gameState.AuctionHouse.PlayersWhoPassedPhase.Count + gameState.AuctionHouse.PlayersWhoPassedAuction.Count == gameState.Players.Count - 1)
                    {
                        bool currentPlayerWon = gameState.AuctionHouse.CurrentBidPlayer.Equals(gameState.CurrentPlayer);
                        BuyCard();
                        CleanupAuctionHouse();
                        if (currentPlayerWon)
                        {
                            AdvanceGame();
                        }
                        else
                        {
                            gameState.CurrentBidder = gameState.CurrentPlayer;
                        }
                    }
                    else
                    {
                        gameState.CurrentBidder = GetNextBidder();
                    }
                }
                else
                {
                    throw new Exception("invalid player.");
                }
            }
            else
            {
                throw new Exception("invalid phase.");
            }
            SendUpdates();
        }

        public static void AuctionPassPhase(Player player)
        {
            var validState = player.Cards.Count() > 0;
            if (validState)
            {
                var validPhase = gameState.CurrentPhase == Phase.AuctionPowerPlants;
                if (validPhase)
                {
                    var validPlayer = gameState.CurrentPlayer.Equals(player);
                    if (validPlayer)
                    {
                        gameState.AuctionHouse.PlayersWhoPassedPhase.Add(player);
                        AdvanceGame();
                    }
                    else
                    {
                        throw new Exception("invalid player.");
                    }
                }
                else
                {
                    throw new Exception("invalid phase.");
                }
            }
            else
            {
                throw new Exception("cannot pass first time.");
            }
        }

        public static void BuyResource(Player buyer, ResourceType type, int count)
        {
            var validPhase = gameState.CurrentPhase == Phase.BuyResources;
            if (validPhase)
            {
                var validPlayer = gameState.CurrentPlayer.Equals(buyer);
                var validType = !type.Equals(ResourceType.Mixed);
                if (validPlayer && validType)
                {
                    Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));

                    for (int i = 0; i < count; i++)
                    {
                        int cost = gameState.ResourceMarket.GetCost(type);
                        if (player.Money >= cost)
                        {
                            player.Money -= cost;
                            player.Resources.AvailableResources[(int)type]++;
                            gameState.ResourceMarket.Resources.AvailableResources[(int)type]--;
                        }
                        else
                        {
                            throw new Exception("invalid count.");
                        }
                    }
                }
                else
                {
                    if (!validPlayer)
                        throw new Exception("invalid player.");
                    else
                        throw new Exception("invalid type.");
                }
            }
            else
            {
                throw new Exception("invalid phase.");
            }
            SendUpdates();
        }

        public static void BuyGenerator(Player buyer, City city)
        {
            var validPhase = gameState.CurrentPhase == Phase.BuildGenerators;
            if (validPhase)
            {
                var validPlayer = gameState.CurrentPlayer.Equals(buyer);
                if (validPlayer)
                {
                    if (buyer.Generators > 0)
                    {
                        Player player = gameState.Players.First(x => x.Name.Equals(buyer.Name));
                        int selectedCityIndex = gameState.Map.Cities.FindIndex((item) =>
                        {
                            return (item.Name == city.Name);
                        });
                        var cost = gameState.Map.CalculateCostToNetwork(city, buyer).Length + gameState.Map.NewGeneratorCost(city);
                        var validCity = !gameState.Map.Cities[selectedCityIndex].Generators.Contains(player) &&
                            IsNewGeneratorAllowedInStep(gameState.Map.Cities[selectedCityIndex], gameState.CurrentStep) &&
                            cost <= buyer.Money;
                        if (validCity)
                        {
                            player.Generators--;
                            player.Money -= cost;
                            gameState.Map.Cities[selectedCityIndex].Generators.Add(player);
                            CheckAuctionHouse();
                        }
                        else
                        {
                            throw new Exception("invalid city.");
                        }
                    }
                    else
                    {
                        throw new Exception("no generators left.");
                    }
                }

                else
                {
                    throw new Exception("invalid player.");
                }
            }
            else
            {
                throw new Exception("invalid phase.");
            }
            SendUpdates();
        }
        
        public static void LoadResource(Player player, Card card, ResourceType resource)
        {
            var validCard = player.Cards.Contains(card) && card.LoadedResources.Count() < card.ResourceCost;
            var validResource = IsLoadableResouce(card, resource);
            if (validCard && validResource)
            {
                var gameStateCard = gameState.Players.SelectMany(x => x.Cards).First(x => x.Equals(card));
                var gameStatePlayer = gameState.Players.First(x => x.Cards.Contains(card));

                if (gameStatePlayer.Resources.AvailableResources[(int)resource] > 0)
                {
                    gameStatePlayer.Resources.AvailableResources[(int)resource]--;
                    gameStateCard.LoadResource(resource);
                }
                else
                {
                    throw new Exception("no resources to load.");
                }
            }
            else
            {
                if (!validCard)
                    throw new Exception("invalid card.");
                else
                    throw new Exception("invalid resource.");
            }
            SendUpdates();
        }
       
        public static void ExportGameStateToJsonFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static void ImportGameStateFromJsonFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            gameState = JsonConvert.DeserializeObject<GameState>(json);
        }
         

        private static void SendUpdates()
        {
            timer?.Dispose();
            foreach (WebSocket listener in listeners)
            {
                var json = JsonConvert.SerializeObject(gameState);
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
                if (!gameState.AuctionHouse.PlayersWhoPassedAuction.Contains(nextBider) && !gameState.AuctionHouse.PlayersWhoBought.Contains(nextBider) && !gameState.AuctionHouse.PlayersWhoPassedPhase.Contains(nextBider))
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
                if (!gameState.AuctionHouse.PlayersWhoBought.Contains(nextPlayer) && !gameState.AuctionHouse.PlayersWhoPassedPhase.Contains(nextPlayer))
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
            var wonCard = gameState.AuctionHouse.Marketplace.Find(x => gameState.AuctionHouse.CurrentAuctionCard.Equals(x));
            winner.Cards.Add(wonCard);
            winner.Money -= gameState.AuctionHouse.CurrentBid;
            gameState.AuctionHouse.Marketplace.Remove(wonCard);
        }

        private static void CleanupAuctionHouse()
        {
            gameState.AuctionHouse.Cleanup();
            gameState.AuctionHouse.Draw(gameState.CurrentStep);
            CheckAuctionHouse();
        }

        private static void CheckAuctionHouse()
        {
            if (gameState.AuctionHouse.DrawPile.Count() == 0)
            {
                if (gameState.CurrentStep != Step.Step3)
                {
                    gameState.CurrentStep = Step.Step3;
                }
            }
            if (IsCheapestCardSmallerThanBiggestGrid())
            {
                gameState.AuctionHouse.RemoveIndex(0);
                gameState.AuctionHouse.Draw(gameState.CurrentStep);
                CheckAuctionHouse();
            }
        }

        private static bool IsCheapestCardSmallerThanBiggestGrid()
        {
            var grids = gameState.Map.Grids;
            if (grids.Any())
            {
                var BiggestGridSize = grids.Select(x => x.Value.Count()).Max();
                if (gameState.AuctionHouse.Marketplace.First().MinimumBid <= BiggestGridSize)
                {
                    return true;
                }
            }
            return false;
        }

        private static int GetIncome(int poweredGenerators)
        {
            switch (poweredGenerators)
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

        private static bool IsNewGeneratorAllowedInStep(City city, Step step)
        {
            if (step == Step.Step1)
                return city.Generators.Count() == 0;
            else if (step == Step.Step2)
                return city.Generators.Count() <= 1;
            else
                return city.Generators.Count() <= 2;
        }

        private static bool IsLoadableResouce(Card card, ResourceType resource)
        {
            if (resource != ResourceType.Mixed)
            {
                if (card.Resource == ResourceType.Mixed)
                    return resource == ResourceType.Oil || resource == ResourceType.Gas;
                else
                    return resource == card.Resource;
            }
            return false;
        }
    }
}
