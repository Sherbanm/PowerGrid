using PowerGrid.Domain;
using System.Collections.Generic;

namespace PowerGridMapDemo
{
    public interface IPositionalGraph
    {
        Dictionary<City, AbstractVector> NodesWithPosition
        {
            get;
        }

        List<Connection> Edges
        {
            get;
        }
    }
}
