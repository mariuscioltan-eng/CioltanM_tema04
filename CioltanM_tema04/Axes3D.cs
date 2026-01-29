using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CioltanM_tema04
{
    class Axes3D
    {
        private bool visible = true;
        private const int AXIS_LENGTH = 100;

        public void Draw()
        {
            if (!visible) return;

            GL.Begin(PrimitiveType.Lines);

            // OX - roșu
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(AXIS_LENGTH, 0, 0);

            // OY - albastru
            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, AXIS_LENGTH, 0);

            // OZ - galben
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, AXIS_LENGTH);

            GL.End();
        }

        public void ToggleVisibility()
        {
            visible = !visible;
        }
    }
}
