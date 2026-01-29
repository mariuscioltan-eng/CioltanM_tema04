using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CioltanM_tema04
{
    class Window3D : GameWindow
    {
        private KeyboardState lastKeyboard;
        private MouseState lastMouse;

        private Axes3D axes;
        private Grid3D grid;
        private Camera3D cam;
        private List<Object3D> objects;
        private Randomizer rnd;

        // control cameră cu mouse dreapta (rotire yaw)
        private bool draggingYaw = false;
        private float lastMouseX;
        private const float YAW_SENSITIVITY = 0.01f;

        private readonly Color BACKGROUND_COLOR = Color.DarkSlateGray;

        public Window3D()
            : base(800, 600, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            rnd = new Randomizer();
            cam = new Camera3D();
            axes = new Axes3D();
            grid = new Grid3D();
            objects = new List<Object3D>();

            HelpMenu();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.ClearColor(BACKGROUND_COLOR);
            GL.Viewport(0, 0, this.Width, this.Height);

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)this.Width / (float)this.Height,
                1,
                1024
            );

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            cam.SetCamera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            //
            // === CAMERA: mouse dreapta pentru yaw ===
            //
            if (currentMouse[MouseButton.Right] && !lastMouse[MouseButton.Right])
            {
                draggingYaw = true;
                lastMouseX = currentMouse.X;
            }

            if (!currentMouse[MouseButton.Right] && lastMouse[MouseButton.Right])
            {
                draggingYaw = false;
            }

            if (draggingYaw)
            {
                float dx = currentMouse.X - lastMouseX;
                cam.YawHorizontal(-dx * YAW_SENSITIVITY);
                lastMouseX = currentMouse.X;
            }

            // scroll = zoom
            float wheelDelta = currentMouse.WheelPrecise - lastMouse.WheelPrecise;
            if (Math.Abs(wheelDelta) > 0.0001f)
            {
                cam.ZoomDistance(-wheelDelta * 2.0f);
            }

            //
            // === CAMERA: tastatură ===
            //
            if (currentKeyboard[Key.W]) { cam.MoveForward(); }
            if (currentKeyboard[Key.S]) { cam.MoveBackward(); }
            if (currentKeyboard[Key.A]) { cam.MoveLeft(); }
            if (currentKeyboard[Key.D]) { cam.MoveRight(); }
            if (currentKeyboard[Key.Q]) { cam.MoveUp(); }
            if (currentKeyboard[Key.E]) { cam.MoveDown(); }

            if (currentKeyboard[Key.Z]) { cam.ZoomDistance(+1.0f); }
            if (currentKeyboard[Key.X]) { cam.ZoomDistance(-1.0f); }

            if (currentKeyboard[Key.N] && !lastKeyboard.Equals(currentKeyboard))
            {
                cam.Near();
            }

            if (currentKeyboard[Key.F] && !lastKeyboard.Equals(currentKeyboard))
            {
                cam.FarAway();
            }

            if (currentKeyboard[Key.R] && !lastKeyboard.Equals(currentKeyboard))
            {
                cam.ResetCamera();
                GL.ClearColor(BACKGROUND_COLOR);
            }

            if (currentKeyboard[Key.B] && !lastKeyboard.Equals(currentKeyboard))
            {
                GL.ClearColor(rnd.RandomColor());
            }

            if (currentKeyboard[Key.M] && !lastKeyboard.Equals(currentKeyboard))
            {
                axes.ToggleVisibility();
            }

            if (currentKeyboard[Key.K] && !lastKeyboard.Equals(currentKeyboard))
            {
                grid.ToggleVisibility();
            }

            if (currentKeyboard[Key.H] && !lastKeyboard.Equals(currentKeyboard))
            {
                HelpMenu();
            }

            if (currentKeyboard[Key.Escape])
            {
                Exit();
            }

            //
            // === OBIECTE 3D ===
            //
            // CLICK STÂNGA: adaugă un nou obiect în cadranul III (X<0, Z<0)
            // Obiectul este un paralelipiped (cub/dreptunghi) desenat cu QUADS.
            if (currentMouse[MouseButton.Left] && !lastMouse.Equals(currentMouse))
            {
                objects.Add(new Object3D());
            }

            // CLICK MIJLOC: șterge toate obiectele
            if (currentMouse[MouseButton.Middle] && !lastMouse.Equals(currentMouse))
            {
                objects.Clear();
            }

            // O: ascunde/afișează toate obiectele
            if (currentKeyboard[Key.O] && !lastKeyboard.Equals(currentKeyboard))
            {
                foreach (var obj in objects)
                {
                    obj.ToggleVisibility();
                }
            }

            // actualizăm stările inputului
            lastKeyboard = currentKeyboard;
            lastMouse = currentMouse;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            grid.Draw();
            axes.Draw();

            foreach (var obj in objects)
            {
                obj.Draw();
            }

            foreach (var obj in objects)
            {
                obj.FallObject();
            }

            SwapBuffers();
        }

        public void HelpMenu()
        {
            Console.Clear();

            Console.WriteLine(
                "      MENIU     \n" +
                "Camera:\n" +
                "  W A S D  - miscare camera pe orizontala\n" +
                "  Q / E    - miscare camera pe verticala\n" +
                "  Click Dreapta + misc mouse stanga/dreapta - roteste camera \n" +
                "  Scroll / Z / X - zoom \n" +
                "  N / F    - preset camera aproape / departe\n" +
                "  R        - reset camera + fundal implicit\n" +
                "  B        - culoare random background\n" +
                "\nScena:\n" +
                "  K        - afiseaza/ascunde grid\n" +
                "  M        - afiseaza/ascunde axe\n" +
                "  O        - afiseaza/ascunde toate obiectele\n" +
                "  Click Stanga   - adauga un obiect plasat RANDOM care cade pana la sol \n" +
                "  Click Mijloc   - sterge toate obiectele\n" +
                "\nGeneral:\n" +
                "  H        - afiseaza meniul\n" +
                "  ESC      - inchidere program\n"
            );
        }
    }
}
