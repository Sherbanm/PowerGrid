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

        private int nextNodeId = 0;
        private int nextEdgeId = 0;

        public City CreateNode(string label, string region)
        {
            City tNewNode = new City()
            {
                ID = nextNodeId.ToString(),
                Name = label,
                Region = region
            };
            nextNodeId++;
            if (!Cities.Contains(tNewNode))
                Cities.Add(tNewNode);
            return tNewNode;
        }

        public Connection CreateEdge(City iSource, City iTarget, int length = 1, string label = "")
        {
            if (iSource == null || iTarget == null)
                return null;

            Connection tNewEdge = new Connection()
            {
                ID = nextEdgeId.ToString(),
                CityA = iSource,
                CityB = iTarget,
                Length = length
            };
            nextEdgeId++;
            if (!Connections.Contains(tNewEdge))
                Connections.Add(tNewEdge);
            return tNewEdge;
        }

        public City GetNode(string label)
        {
            return Cities.FirstOrDefault(x => x.Name.Equals(label));
        }

        public Connection GetEdge(string label)
        {
            return Connections.FirstOrDefault(x => (x.CityA.Name + "-" + x.CityB.Name).Equals(label));
        }
        
        public List<Connection> GetEdges(City iNode1, City iNode2)
        {
            return Connections.Where(x => ( x.CityA == iNode1 && x.CityB == iNode2 ) || (x.CityA == iNode2 && x.CityB == iNode1)).ToList();
        }

        public List<Connection> GetEdges(City iNode)
        {
            return Connections.Where(x => x.CityA == iNode || x.CityB == iNode).ToList();
        }

        public void RemoveNode(City iNode)
        {
            if (Cities.Contains(iNode))
            {
                Cities.Remove(iNode);
            }
            DetachNode(iNode);
        }

        public void RemoveEdge(Connection iEdge)
        {
            Connections.Remove(iEdge);
        }

        public void Clear()
        {
            Cities.Clear();
            Connections.Clear();
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
                        var relevantEdge = GetEdges(initialState.Key, neighbourNode).FirstOrDefault();
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

        
        private void DetachNode(City iNode)
        {
            Connections.ForEach(delegate (Connection e)
            {
                if (e.CityA.ID == iNode.ID || e.CityB.ID == iNode.ID)
                {
                    RemoveEdge(e);
                }
            });
        }

        private List<City> GetNeighbours(City node)
        {
            List<City> neighbours = new List<City>();
            var relevantEdges = GetEdges(node);
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
