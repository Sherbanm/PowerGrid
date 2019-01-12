using PowerGrid.Domain;
using PowerGrid.Service;
using System;
using Xunit;
using Newtonsoft.Json;

namespace PowerGrid.Tests
{
    public class ResourceMarketTests
    {
        [Fact]
        public void Normal()
        {
            GameState g = MockGameState.GetMockState();
            int cost = g.ResourceMarket.GetCost(ResourceType.Coal);

            string preGameStateString = JsonConvert.SerializeObject(g);
            GameState preGameState = JsonConvert.DeserializeObject<GameState>(preGameStateString);
            
            Game.BuyResource(preGameState.Players[0], ResourceType.Coal, 1);
            GameState postGameState = MockGameState.GetMockState();
            Assert.Equal(preGameState.Players[0].Money, postGameState.Players[0].Money + cost);
        }

        [Fact]
        public void NoMoney()
        {
            GameState g = MockGameState.GetMockState();
            g.Players[0].Money = 0;
            string preGameStateString = JsonConvert.SerializeObject(g);
            GameState preGameState = JsonConvert.DeserializeObject<GameState>(preGameStateString);

            Game.BuyResource(preGameState.Players[0], ResourceType.Coal, 1);
            GameState postGameState = MockGameState.GetMockState();
            Assert.Equal(preGameState.Players[0].Money, postGameState.Players[0].Money);
            Assert.Equal(JsonConvert.SerializeObject(preGameState.ResourceMarket), JsonConvert.SerializeObject(postGameState.ResourceMarket));
        }

        [Fact]
        public void NoResources()
        {
            for (int i = 0; i <= (int)ResourceType.Nuclear; i++)
            {
                GameState g = MockGameState.GetMockState();
                g.ResourceMarket.Resources.Data[i] = 0;
                string preGameStateString = JsonConvert.SerializeObject(g);
                GameState preGameState = JsonConvert.DeserializeObject<GameState>(preGameStateString);

                Game.BuyResource(preGameState.Players[0], (ResourceType)i, 1);
                GameState postGameState = MockGameState.GetMockState();
                Assert.Equal(preGameState.Players[0].Money, postGameState.Players[0].Money);
                Assert.Equal(JsonConvert.SerializeObject(preGameState.ResourceMarket), JsonConvert.SerializeObject(postGameState.ResourceMarket)); Assert.Equal(JsonConvert.SerializeObject(preGameState.ResourceMarket), JsonConvert.SerializeObject(postGameState.ResourceMarket));
            } 
        }

        [Fact]
        public void BuyAllResources()
        {
            GameState g = MockGameState.GetMockState();
            g.Players[0].Money = int.MaxValue; // "infinite money"

            for (int i = 0; i <= (int)ResourceType.Nuclear; i++)
            {
                int maxSupply = g.ResourceMarket.GetMaxSuppy((ResourceType)i);
                g.ResourceMarket.Resources.Data[i] = maxSupply;
                for (int j = 0; j < maxSupply; j++)
                {
                    string preGameStateString = JsonConvert.SerializeObject(g);
                    GameState preGameState = JsonConvert.DeserializeObject<GameState>(preGameStateString);
                    int cost = g.ResourceMarket.GetCost((ResourceType)i);
                    
                    Game.BuyResource(preGameState.Players[0], (ResourceType)i, 1);
                    GameState postGameState = MockGameState.GetMockState();
                    Assert.Equal(preGameState.Players[0].Money, postGameState.Players[0].Money + cost);
                    Assert.Equal(preGameState.ResourceMarket.Resources.Data[i], postGameState.ResourceMarket.Resources.Data[i] + 1);
                }
            }
        }
    }
}
