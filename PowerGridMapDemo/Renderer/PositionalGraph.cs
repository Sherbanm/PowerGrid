using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGridMapDemo
{
    public class PositionalGraph : Map
    {
        public Dictionary<City, GridBox> NodesWithGridBox { get; set; } = new Dictionary<City, GridBox>();

        public Dictionary<Connection, GridLine> ConnectionsWithGridLines { get; set; } = new Dictionary<Connection, GridLine>();

        public PositionalGraph()
        {
        }

        public City CreateNode(string label, string region, GridBox gridBox)
        {
            var newNode = base.CreateNode(label, region);
            NodesWithGridBox.Add(newNode, gridBox);
            return newNode;
        }

        public Connection CreateEdge(City iSource, City iTarget, int length , string label, GridLine gridLine)
        {
            var connection = base.CreateEdge(iSource, iTarget, length, label);
            ConnectionsWithGridLines.Add(connection, gridLine);
            return connection;
        }
    }
}
