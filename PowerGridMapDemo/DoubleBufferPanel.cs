using System.Windows.Forms;
namespace PowerGridMapDemo
{
    class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel() : base()
        {
            this.DoubleBuffered = true;
            this.UpdateStyles();
        }
    }
}
