using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CioltanM_tema04
{
    class Grid3D
    {
        private bool visible = true;

        private readonly Color GRID_COLOR = Color.WhiteSmoke;

        private const int GRIDSTEP = 10;
        private const int UNITS = 50;
        private const int POINT_OFFSET = GRIDSTEP * UNITS;
        private const int MICRO_OFFSET = 1;

        public void Draw()
        {
            if (!visible) return;

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(GRID_COLOR);

            for (int i = -1 * GRIDSTEP * UNITS; i <= GRIDSTEP * UNITS; i += GRIDSTEP)
            {
                GL.Vertex3(i + MICRO_OFFSET, 0, POINT_OFFSET);
                GL.Vertex3(i + MICRO_OFFSET, 0, -POINT_OFFSET);

                GL.Vertex3(POINT_OFFSET, 0, i + MICRO_OFFSET);
                GL.Vertex3(-POINT_OFFSET, 0, i + MICRO_OFFSET);
            }

            GL.End();
        }

        public void ToggleVisibility()
        {
            visible = !visible;
        }
    }
}
