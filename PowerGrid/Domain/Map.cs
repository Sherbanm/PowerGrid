using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PowerGrid.Domain
{
    public class Map
    {
        [JsonProperty(PropertyName = "connections")]
        public List<Connection> Connections { get; set; } = new List<Connection>();

        [JsonProperty(PropertyName = "cities")]
        public List<City> Cities { get; set; } = new List<City>();


        private Dictionary<string, City> m_nodeSet = new Dictionary<string, City>();
        private Dictionary<string, Dictionary<string, List<Connection>>> m_adjacencySet = new Dictionary<string, Dictionary<string, List<Connection>>>();
        private int m_nextNodeId = 0;
        private int m_nextEdgeId = 0;

        public void Clear()
        {
            Cities.Clear();
            Connections.Clear();
            m_adjacencySet.Clear();
        }
        public virtual City AddNode(City iNode)
        {
            if (!m_nodeSet.ContainsKey(iNode.ID))
            {
                Cities.Add(iNode);
            }

            m_nodeSet[iNode.ID] = iNode;
            return iNode;
        }

        public Connection AddEdge(Connection iEdge)
        {
            if (!Connections.Contains(iEdge))
                Connections.Add(iEdge);


            if (!(m_adjacencySet.ContainsKey(iEdge.CityA.ID)))
            {
                m_adjacencySet[iEdge.CityA.ID] = new Dictionary<string, List<Connection>>();
            }
            if (!(m_adjacencySet[iEdge.CityA.ID].ContainsKey(iEdge.CityB.ID)))
            {
                m_adjacencySet[iEdge.CityA.ID][iEdge.CityB.ID] = new List<Connection>();
            }


            if (!m_adjacencySet[iEdge.CityA.ID][iEdge.CityB.ID].Contains(iEdge))
            {
                m_adjacencySet[iEdge.CityA.ID][iEdge.CityB.ID].Add(iEdge);
            }

            return iEdge;
        }

        public City CreateNode(string label, string region)
        {
            City tNewNode = new City()
            {
                ID = m_nextNodeId.ToString(),
                Name = label,
                Region = region
            };
            m_nextNodeId++;
            AddNode(tNewNode);
            return tNewNode;
        }

        public Connection CreateEdge(City iSource, City iTarget, int length = 1, string label = "")
        {
            if (iSource == null || iTarget == null)
                return null;

            Connection tNewEdge = new Connection()
            {
                ID = m_nextEdgeId.ToString(),
                CityA = iSource,
                CityB = iTarget, 
                Length= length
            };
            m_nextEdgeId++;
            AddEdge(tNewEdge);
            return tNewEdge;
        }

        public List<Connection> GetEdges(City iNode1, City iNode2)
        {
            if (m_adjacencySet.ContainsKey(iNode1.ID) && m_adjacencySet[iNode1.ID].ContainsKey(iNode2.ID))
            {
                return m_adjacencySet[iNode1.ID][iNode2.ID];
            }
            return null;
        }

        public List<Connection> GetEdges(City iNode)
        {
            List<Connection> retEdgeList = new List<Connection>();
            if (m_adjacencySet.ContainsKey(iNode.ID))
            {
                foreach (KeyValuePair<string, List<Connection>> keyPair in m_adjacencySet[iNode.ID])
                {
                    foreach (Connection e in keyPair.Value)
                    {
                        retEdgeList.Add(e);
                    }
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, List<Connection>>> keyValuePair in m_adjacencySet)
            {
                if (keyValuePair.Key != iNode.ID)
                {
                    foreach (KeyValuePair<string, List<Connection>> keyPair in m_adjacencySet[keyValuePair.Key])
                    {
                        foreach (Connection e in keyPair.Value)
                        {
                            retEdgeList.Add(e);
                        }
                    }

                }
            }
            return retEdgeList;
        }

        public void RemoveNode(City iNode)
        {
            if (m_nodeSet.ContainsKey(iNode.ID))
            {
                m_nodeSet.Remove(iNode.ID);
            }
            Cities.Remove(iNode);
            DetachNode(iNode);

        }
        
        public void DetachNode(City iNode)
        {
            Connections.ForEach(delegate (Connection e)
            {
                if (e.CityA.ID == iNode.ID || e.CityB.ID == iNode.ID)
                {
                    RemoveEdge(e);
                }
            });
        }

        public void RemoveEdge(Connection iEdge)
        {
            Connections.Remove(iEdge);
            foreach (KeyValuePair<string, Dictionary<string, List<Connection>>> x in m_adjacencySet)
            {
                foreach (KeyValuePair<string, List<Connection>> y in x.Value)
                {
                    List<Connection> tEdges = y.Value;
                    tEdges.Remove(iEdge);
                    if (tEdges.Count == 0)
                    {
                        m_adjacencySet[x.Key].Remove(y.Key);
                        break;
                    }
                }
                if (x.Value.Count == 0)
                {
                    m_adjacencySet.Remove(x.Key);
                    break;
                }
            }
        }

        public City GetNode(string label)
        {
            City retNode = null;
            Cities.ForEach(delegate (City n)
            {
                if (n.Name == label)
                {
                    retNode = n;
                }
            });
            return retNode;
        }

        public Connection GetEdge(string label)
        {
            Connection retEdge = null;
            Connections.ForEach(delegate (Connection e)
            {
                if (e.CityA.Name + "-" + e.CityB.Name == label)
                {
                    retEdge = e;
                }
            });
            return retEdge;
        }

        public Path CalculateCostToNetwork(City initialNode)
        {
            List<City> goalNodes = Cities.Where(x => x.Build).ToList();
            var shortestDistance = int.MaxValue;
            Path shortestPath = new Path();

            foreach (var goalNode in goalNodes)
            {
                List<City> visitedNodes = new List<City>();
                Dictionary<City, Path> tentativeDistances = new Dictionary<City, Path>();
                foreach (var node in Cities)
                {
                    tentativeDistances.Add(node, new Path() { Length = int.MaxValue });
                }
                tentativeDistances[initialNode].Length = 0;

                while (!visitedNodes.Contains(goalNode))
                {
                    var initialState = tentativeDistances.Where(x => !visitedNodes.Contains(x.Key)).OrderBy(x => x.Value.Length).First();
                    visitedNodes.Add(initialState.Key);

                    var neighbourNodes = GetNeighbours(initialState.Key);
                    var relevantNeighbourNodes = neighbourNodes.Where(x => !visitedNodes.Contains(x));
                    foreach (var neighbourNode in relevantNeighbourNodes)
                    {
                        var relevantEdge = Connections.FirstOrDefault(x =>
                        (x.CityA.Equals(initialState.Key) && x.CityB.Equals(neighbourNode)) ||
                        (x.CityA.Equals(neighbourNode) && x.CityB.Equals(initialState.Key)));
                        var edgeWeight = relevantEdge.Length;
                        if (edgeWeight + initialState.Value.Length < tentativeDistances[neighbourNode].Length)
                        {
                            tentativeDistances[neighbourNode].Length = edgeWeight + initialState.Value.Length;
                            var newPath = initialState.Value.Edges.ToList();
                            newPath.Add(relevantEdge);
                            tentativeDistances[neighbourNode].Edges = newPath;
                        }
                    }
                }
                if (tentativeDistances[goalNode].Length < shortestDistance)
                {
                    shortestDistance = tentativeDistances[goalNode].Length;
                    shortestPath = tentativeDistances[goalNode];
                }
            }
            return shortestPath;
        }

        public List<City> GetNeighbours(City node)
        {
            List<City> neighbours = new List<City>();
            var relevantEdges = Connections.Where(x => x.CityA.Equals(node) || x.CityB.Equals(node));
            foreach (var edge in relevantEdges)
            {
                neighbours.Add(edge.CityA);
                neighbours.Add(edge.CityB);
            }
            return neighbours.Distinct().Where(x => x != node).ToList();
        }
    }

    public class Path
    {
        public List<Connection> Edges = new List<Connection>();

        public int Length { get; set; }
    }
}
