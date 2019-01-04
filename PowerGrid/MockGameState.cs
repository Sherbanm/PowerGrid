using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGrid
{
    public static class MockGameState
    {
        public static Card[] cards = new Card[] { new Card { Cost = 1, Resource = ResourceType.Coal, Value = 2, Power = 3 },
                                new Card { Cost = 5, Resource = ResourceType.Oil, Value = 6, Power = 7 }};

        public static Card[] cards2 = new Card[] { new Card { Cost = 11, Resource = ResourceType.Green, Value = 12, Power = 13 },
                                new Card { Cost = 15, Resource = ResourceType.Mixed, Value = 16, Power = 17 },
                                new Card { Cost = 5, Resource = ResourceType.Nuclear, Value = 6, Power = 7 }};
            
        public static Player[] players = new Player[] {
                new Player { Name = "steve", Cards = cards    },
                new Player { Name = "stove", Cards = cards2   } };


        public static GameState GetMockState() {
            players[0].Cards[0].Cost++;
            return new GameState { Players = players };
        }

        public static Player[] GetMockPlayers()
        {
            return players;
        }

        public static Card[] GetMockCards()
        {
            return cards;
        }
    }
}
