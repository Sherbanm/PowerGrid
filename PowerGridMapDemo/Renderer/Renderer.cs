using PowerGrid.Domain;
using System;
using System.Collections.Generic;

namespace PowerGridMapDemo
{
    class Renderer : AbstractRenderer
    {
        public Dictionary<City, GridBox> Boxes { get; set; }

        public Dictionary<Connection, GridLine> Lines { get;set; }

        int Height;
        int Width;

        public Renderer(IForceDirected iForceDirected, int height, int width)
            : base(iForceDirected)
        {
            Boxes = new Dictionary<City, GridBox>();
            Lines = new Dictionary<Connection, GridLine>();
            Height = height;
            Width = width;
        }

        public override void Clear()
        {
        }

        protected override void drawEdge(Connection iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
        {
            Tuple<int, int> pos1 = GraphToScreen(iPosition1 as FDGVector2);
            Tuple<int, int> pos2 = GraphToScreen(iPosition2 as FDGVector2);
            Lines[iEdge].Set(pos1.Item1, pos1.Item2, pos2.Item1, pos2.Item2);
            Lines[iEdge].DrawLine(Paper, iEdge);
        }

        protected override void drawNode(City iNode, AbstractVector iPosition)
        {
            Tuple<int, int> pos = GraphToScreen(iPosition as FDGVector2);
            Boxes[iNode].Set(pos.Item1, pos.Item2);
            Boxes[iNode].DrawBox(Paper, iNode);
        }

        public Tuple<int, int> GraphToScreen(FDGVector2 iPos)
        {
            var x = (int)(iPos.x + (((Width)) / 2.0f));
            var y = (int)(iPos.y + (((Height)) / 2.0f));
            Tuple<int, int> retPair = new Tuple<int, int>(x, y);
            return retPair;
        }

        public FDGVector2 ScreenToGraph(Tuple<int, int> iScreenPos)
        {
            FDGVector2 retVec = new FDGVector2();
            retVec.x = (iScreenPos.Item1) - (((Width)) / 2.0f);
            retVec.y = (iScreenPos.Item2) - (((Height)) / 2.0f);
            return retVec;
        }
    }
}
