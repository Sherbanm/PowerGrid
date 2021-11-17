using PowerGrid.Domain;
using System.Collections.Generic;
using System.IO;

using System.Xml;

namespace PowerGrid
{
    public static class MockGameState
    {
        public static List<Card> cards = new List<Card> { new Card { Cost = 1, Resource = ResourceType.Coal, Value = 2, Power = 3 },
                                new Card { Cost = 5, Resource = ResourceType.Oil, Value = 6, Power = 7 }};

        public static List<Card> cards2 = new List<Card> { new Card { Cost = 11, Resource = ResourceType.Green, Value = 12, Power = 13 },
                                new Card { Cost = 15, Resource = ResourceType.Mixed, Value = 16, Power = 17 },
                                new Card { Cost = 5, Resource = ResourceType.Nuclear, Value = 6, Power = 7 }};

        public static Player[] players = new Player[] {
                new Player { Name = "Albert", Money = 50 },
                new Player { Name = "Bruce", Money = 50 },
                new Player { Name = "Charles", Money = 50 },
                new Player { Name = "Dan", Money = 50 },
                new Player { Name = "Eustache", Money = 50 },
                new Player { Name = "Franz", Money = 50 },
        };

        public static ResourceMarket resourceMarket = new ResourceMarket
        {
            Resources = new Resources {
                Data = new int[] { 10, 12, 15, 5 }
            }
        };

        public static City Toronto = new City { Name = "Toronto"};
        public static City Montreal = new City { Name = "Montreal"};
        public static City Ottawa = new City { Name = "Ottawa"};

        // public static Map map = new Map
        // {
        //     Cities = new List<City> { Toronto, Montreal, Ottawa },
        //     Connections = new List<Connection>
        //     {
        //         new Connection { CityA = Toronto, CityB = Montreal, Length = 5},
        //         new Connection { CityA = Toronto, CityB = Ottawa, Length = 4 },
        //         new Connection { CityA = Montreal, CityB = Ottawa, Length = 2 }
        //     }
        // };

        private static Map map = null;

        public static Map Map
        {
            get
            {
                if (map == null)
                {
                    map = new Map();
                    try
                    {
                        string text = File.ReadAllText("D:\\Desktop\\PowerGridGraph.xml");
                        int size = text.Length;

                        StringReader mapXML = new StringReader(text);
                        XmlTextReader xmlReader = new XmlTextReader(mapXML);
                        while (xmlReader.Read())
                        {
                            switch (xmlReader.NodeType)
                            {
                                case XmlNodeType.Element: // The node is an Element.
                                    if (xmlReader.Name == "Node")
                                    {
                                        LoadNode(map, xmlReader);
                                    }
                                    else if (xmlReader.Name == "Edge")
                                    {
                                        LoadEdge(map, xmlReader);
                                    }
                                    break;

                                case XmlNodeType.Text: //Display the text in each element.
                                    break;
                                case XmlNodeType.EndElement: //Display end of element.
                                    break;
                                case XmlNodeType.Whitespace: //Display end of element.
                                    break;
                            }
                        }
                    }
                    catch (IOException)
                    {
                    }
                }
                return map;
            }
        }

        private static void LoadNode(Map map, XmlTextReader iXmlReader)
        {
            string name = string.Empty;
            string region = string.Empty;

            while (iXmlReader.MoveToNextAttribute()) // Read attributes.
            {
                if (iXmlReader.Name == "nodeName")
                    name = iXmlReader.Value;
                if (iXmlReader.Name == "region")
                    region = iXmlReader.Value;
            }
            map.CreateNode(name.Trim(), region);
        }

        private static void LoadEdge(Map map,XmlTextReader iXmlReader)
        {
            string nodeName1 = null;
            string nodeName2 = null;
            int length = 0;
            while (iXmlReader.MoveToNextAttribute()) // Read attributes.
            {
                if (iXmlReader.Name == "nodeName1")
                    nodeName1 = iXmlReader.Value;
                if (iXmlReader.Name == "nodeName2")
                    nodeName2 = iXmlReader.Value;
                if (iXmlReader.Name == "length")
                    length = int.Parse(iXmlReader.Value);
            }
            City node1 = Map.GetNode(nodeName1.Trim());
            City node2 = Map.GetNode(nodeName2.Trim());
            string label = nodeName1 + "-" + nodeName2;
            map.CreateEdge(node1, node2, length, label);
        }

        public static GameState GetMockState() {
            return new GameState {
                Players = players,
                ResourceMarket = resourceMarket,
                AuctionHouse = new AuctionHouse { AvailableCards = cards, DisplayedCards = cards2 },
                Map = Map,
                CurrentPlayer = players[0],
                RemainingSeconds = 30
            };
        }

        public static Player[] GetMockPlayers()
        {
            return players;
        }

        public static List<Card> GetMockCards()
        {
            return cards;
        }

    }
}
