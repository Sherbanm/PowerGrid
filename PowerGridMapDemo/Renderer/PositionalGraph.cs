using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGridMapDemo
{
    public class PositionalGraph : Map
    {
        public Dictionary<City, AbstractVector> NodesWithPosition { get; set; } = new Dictionary<City, AbstractVector>();

        public PositionalGraph()
        {
        }

        public override City AddNode(City node)
        {
            var newNode = base.AddNode(node);
            NodesWithPosition.Add(newNode, FDGVector2.Random() as FDGVector2);
            return newNode;
        }
    }
}
