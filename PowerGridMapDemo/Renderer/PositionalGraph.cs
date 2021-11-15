using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGridMapDemo
{
    public class PositionalGraph : Map
    {
        public Dictionary<City, GridBox> NodesWithGridBox { get; set; } = new Dictionary<City, GridBox>();

        public PositionalGraph()
        {
        }

        public City CreateNode(string label, string region, GridBox gridBox)
        {
            var newNode = base.CreateNode(label, region);
            NodesWithGridBox.Add(newNode, gridBox);
            return newNode;
        }
    }
}
