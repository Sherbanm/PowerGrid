﻿using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace PowerGrid
{
    public static class MockGameState
    {
        private static List<Card> cards = null;

        public static List<Card> Cards
        {
            get
            {
                if (cards == null)
                {
                    cards = new List<Card>();
                    try
                    {
                        string text = File.ReadAllText("D:\\Desktop\\PowerGridCards.xml");
                        int size = text.Length;

                        StringReader mapXML = new StringReader(text);
                        XmlTextReader xmlReader = new XmlTextReader(mapXML);
                        while (xmlReader.Read())
                        {
                            switch (xmlReader.NodeType)
                            {
                                case XmlNodeType.Element: // The node is an Element.
                                    if (xmlReader.Name == "Card")
                                    {
                                        LoadCard(xmlReader);
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
                return cards;
            }
        }

        private static void LoadCard(XmlTextReader iXmlReader)
        {
            var card = new Card();

            while (iXmlReader.MoveToNextAttribute()) // Read attributes.
            {
                if (iXmlReader.Name == "minimumBid")
                    card.MinimumBid = int.Parse(iXmlReader.Value);
                if (iXmlReader.Name == "resource")
                    card.Resource = (ResourceType)Enum.Parse(typeof(ResourceType), iXmlReader.Value);
                if (iXmlReader.Name == "resourceCost")
                    card.ResourceCost = int.Parse(iXmlReader.Value);
                if (iXmlReader.Name == "generatorsPowered")
                    card.GeneratorsPowered = int.Parse(iXmlReader.Value);
            }
            cards.Add(card);
        }


        public static List<Card> cards2 = new List<Card> { new Card { ResourceCost = 11, Resource = ResourceType.Green, MinimumBid = 12, GeneratorsPowered = 13 },
                                new Card { ResourceCost = 15, Resource = ResourceType.Mixed, MinimumBid = 16, GeneratorsPowered = 17 },
                                new Card { ResourceCost = 5, Resource = ResourceType.Nuclear, MinimumBid = 6, GeneratorsPowered = 7 }};

        public static LinkedList<Player> players = new LinkedList<Player>(new List<Player> {
                new Player { Name = "Albert", Money = 50 },
                new Player { Name = "Bruce", Money = 50 },
                new Player { Name = "Charles", Money = 50 },
                new Player { Name = "Dan", Money = 50 },
                new Player { Name = "Eustache", Money = 50 },
                new Player { Name = "Franz", Money = 50 },
        });

        public static ResourceMarket resourceMarket = new ResourceMarket
        {
            Resources = new Resources {
                AvailableResources = new int[] { 23, 18, 14, 1 }
            }
        };

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
            var gameState =  new GameState {
                Players = players,
                ResourceMarket = resourceMarket,
                AuctionHouse = new AuctionHouse(),
                Map = Map,
                RemainingTime = 10
            };
            gameState.AuctionHouse.Prepare(Cards.Where(x => x.MinimumBid <= 15), Cards.Where(x => x.MinimumBid > 15), true, players.Count());
            return gameState;

        }

        public static LinkedList<Player> GetMockPlayers()
        {
            return players;
        }

        public static List<Card> GetMockCards()
        {
            return cards;
        }

    }
}
