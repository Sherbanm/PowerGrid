
using System.Windows.Forms;

namespace PowerGridMapDemo
{

    public interface IRenderer
    {

        void Clear();
        void Draw(float iTimeStep, PaintEventArgs e);
    }
}
