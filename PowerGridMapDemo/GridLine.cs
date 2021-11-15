using PowerGrid.Domain;
using System.Drawing;

namespace PowerGridMapDemo
{
    class GridLine
    {
        public int fromX, fromY, toX, toY;
        public Pen pen;
        
        public GridLine(int iFromX, int iFromY, int iToX,int iToY)
        {
            this.fromX = iFromX + 9;
            this.fromY = iFromY + 9;
            this.toX = iToX + 9;
            this.toY = iToY + 9;
            pen = new Pen(Color.Yellow);
            pen.Width = 2;
            
            
        }
        public void Set(int iFromX, int iFromY, int iToX,int iToY)
        {
            this.fromX = iFromX + 9;
            this.fromY = iFromY + 9;
            this.toX = iToX + 9;
            this.toY = iToY + 9;
        }
        public void DrawLine(Graphics iPaper, Connection edge)
        {
            var color = Color.Yellow;
            var thickness = 2;
            if (edge.Highlight)
            {
                color = Color.BlueViolet;
                thickness = 4;
            }
            var newPen = new Pen(color, thickness);
            iPaper.DrawLine(newPen, fromX, fromY, toX, toY);
            
        }


        public void Dispose()
        {
            if (this.pen != null)
                this.pen.Dispose();

        }
    }
}
