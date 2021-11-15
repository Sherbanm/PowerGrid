using PowerGrid.Domain;

namespace PowerGridMapDemo
{
    public abstract class AbstractRenderer : IRenderer
    {
        public IForceDirected forceDirected;
        public AbstractRenderer(IForceDirected iForceDirected)
        {
            forceDirected = iForceDirected;
        }


        public void Draw(float iTimeStep)
        {
            forceDirected.Calculate(iTimeStep);
            Clear();
            forceDirected.EachEdge(delegate(Connection edge, Spring spring)
            {
                drawEdge(edge, spring.point1.position, spring.point2.position);
            });
            forceDirected.EachNode(delegate(City node, Point point)
            {
                drawNode(node, point.position);
            });
        }
        public abstract void Clear();
        protected abstract void drawEdge(Connection iEdge, AbstractVector iPosition1, AbstractVector iPosition2);
        protected abstract void drawNode(City iNode, AbstractVector iPosition);

    }
}
