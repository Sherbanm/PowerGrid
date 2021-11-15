using PowerGrid.Domain;
using System;
using System.Drawing;

namespace PowerGridMapDemo
{
    enum BoxType { Normal,Pinned};

    class GridBox : IDisposable
    {
        public int x, y, width, height;
        public SolidBrush brush;
        public Rectangle boxRec;
        public BoxType boxType;
        public GridBox(int iX, int iY,BoxType iType)
        {
            this.x = iX;
            this.y = iY;
            this.boxType = iType;
            switch (iType)
            {
                case BoxType.Normal:
                    brush = new SolidBrush(Color.Black);
                    break;
                case BoxType.Pinned:
                    brush = new SolidBrush(Color.Red);
                    break;
            
            }
            width = 18;
            height = 18;
            boxRec = new Rectangle(x, y, width, height);
        }

        public void Set(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
            boxRec.X = x;
            boxRec.Y = y;
        }
        public void DrawBox(Graphics iPaper, City node)
        {
            var label = string.Empty;
            var labelColor = Color.CadetBlue;
            if (node.Label.Length > 0)
            {
                label = node.Label.Substring(0, 1);
            }
            // string name, string region, bool build
            if (boxType == BoxType.Pinned)
            {
                boxRec.Width = 26;
                boxRec.Height = 26;
                label = node.Cost.ToString();
                labelColor = Color.IndianRed;
            }
            else
            {
                boxRec.Width = 18;
                boxRec.Height = 18;
            }
            SetBrush(node);
            iPaper.FillRectangle(brush, boxRec);
            iPaper.DrawString(label, new Font("Arial", 16), new SolidBrush(labelColor), boxRec.X, boxRec.Y);
        }

        public void SetBrush(City node)
        {
            var region = node.Region;
            Color color = Color.Black;
            if (region == "yellow")
                color = Color.Yellow;
            else if (region == "blue")
                color = Color.Blue;
            else if (region == "purple")
                color = Color.Purple;
            else if (region == "orange")
                color = Color.Orange;
            else if (region == "red")
                color = Color.Red;
            else if (region == "green")
                color = Color.Green;
            else if (region == "olive")
                color = Color.Olive;

            if (node.Build)
                color = Color.Gray;
            this.brush = new SolidBrush(color);
        }
        
        public void SetNormalBox()
        {
            if (this.brush != null)
                this.brush.Dispose();
           this.brush = new SolidBrush(Color.WhiteSmoke);
           this.boxType = BoxType.Normal;
        }

        public void SetEndBox()
        {
            if (this.brush != null)
                this.brush.Dispose();
            this.brush = new SolidBrush(Color.Red);
            this.boxType = BoxType.Pinned;
        }


        public void Dispose()
        {
            if(this.brush!=null)
                this.brush.Dispose();

        }
    }
}
