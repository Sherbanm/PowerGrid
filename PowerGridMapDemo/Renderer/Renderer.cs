using PowerGrid.Domain;

namespace PowerGridMapDemo
{
    class Renderer:AbstractRenderer
    {
        ForceDirectedGraphForm form;
        public Renderer(ForceDirectedGraphForm iForm,IForceDirected iForceDirected)
            : base(iForceDirected)
        {
            form = iForm;
        }

        public override void Clear()
        {

        }

        protected override void drawEdge(Connection iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
        {
            //TODO: Change positions of line
            
            form.DrawLine(iEdge,iPosition1,iPosition2);
        }

        protected override void drawNode(City iNode, AbstractVector iPosition)
        {
            //TODO: Change positions of line
            form.DrawBox(iNode,iPosition);
        }
    }
}
