using PowerGrid.Domain;

namespace PowerGridMapDemo
{
    public delegate void EdgeAction(Connection edge, Spring spring);
    public delegate void NodeAction(City edge, Point point);

    public interface IForceDirected
    {
        PositionalGraph graph
        {
            get;
        }

        float Stiffness
        {
            get;
        }

        float Repulsion
        {
            get;
        }

        float Damping
        {
            get;
        }

        float Threadshold // NOT Using
        {
            get;
            set;
        }
        bool WithinThreashold
        {
            get;
        }
        void Clear();
        void Calculate(float iTimeStep);
        void EachEdge(EdgeAction del);
        void EachNode(NodeAction del);
        NearestPoint Nearest(AbstractVector position);
        BoundingBox GetBoundingBox();
    }
}
