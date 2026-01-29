using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace CioltanM_tema04
{
    class Camera3D
    {
        private Vector3 eye;       
        private Vector3 target;     
        private Vector3 up_vector;  

        private const float MOVEMENT_UNIT = 2.0f;

        private float yaw;
        private float distanceToTarget;

        public Camera3D()
        {
            eye = new Vector3(70, 70, 70);
            target = new Vector3(0, 0, 0);
            up_vector = new Vector3(0, 1, 0);

            RecomputePolar();
        }

        private void RecomputePolar()
        {
            Vector3 dir = eye - target;
            distanceToTarget = dir.Length;
            if (distanceToTarget < 0.001f)
                distanceToTarget = 0.001f;

            yaw = (float)Math.Atan2(dir.Z, dir.X); 
        }

        public void YawHorizontal(float deltaYaw)
        {
            yaw += deltaYaw;

            float x = distanceToTarget * (float)Math.Cos(yaw);
            float z = distanceToTarget * (float)Math.Sin(yaw);
            float y = eye.Y; 

            eye = new Vector3(
                target.X + x,
                y,
                target.Z + z
            );

            SetCamera();
        }
        public void ZoomDistance(float delta)
        {
            distanceToTarget += delta;
            if (distanceToTarget < 1.0f)
                distanceToTarget = 1.0f;

            float x = distanceToTarget * (float)Math.Cos(yaw);
            float z = distanceToTarget * (float)Math.Sin(yaw);

            eye = new Vector3(
                target.X + x,
                eye.Y,
                target.Z + z
            );

            SetCamera();
        }

        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up_vector);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }
        public void MoveForward()
        {
            eye = new Vector3(eye.X - MOVEMENT_UNIT, eye.Y, eye.Z);
            target = new Vector3(target.X - MOVEMENT_UNIT, target.Y, target.Z);
            RecomputePolar();
            SetCamera();
        }

        public void MoveBackward()
        {
            eye = new Vector3(eye.X + MOVEMENT_UNIT, eye.Y, eye.Z);
            target = new Vector3(target.X + MOVEMENT_UNIT, target.Y, target.Z);
            RecomputePolar();
            SetCamera();
        }

        public void MoveLeft()
        {
            eye = new Vector3(eye.X, eye.Y, eye.Z + MOVEMENT_UNIT);
            target = new Vector3(target.X, target.Y, target.Z + MOVEMENT_UNIT);
            RecomputePolar();
            SetCamera();
        }

        public void MoveRight()
        {
            eye = new Vector3(eye.X, eye.Y, eye.Z - MOVEMENT_UNIT);
            target = new Vector3(target.X, target.Y, target.Z - MOVEMENT_UNIT);
            RecomputePolar();
            SetCamera();
        }

        public void MoveUp()
        {
            eye = new Vector3(eye.X, eye.Y + MOVEMENT_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y + MOVEMENT_UNIT, target.Z);
            RecomputePolar();
            SetCamera();
        }

        public void MoveDown()
        {
            eye = new Vector3(eye.X, eye.Y - MOVEMENT_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y - MOVEMENT_UNIT, target.Z);
            RecomputePolar();
            SetCamera();
        }
        public void Near()
        {
            eye = new Vector3(200, 175, 25);
            target = new Vector3(0, 25, 0);
            RecomputePolar();
            SetCamera();
        }

        public void FarAway()
        {
            eye = new Vector3(400, 175, 225);
            target = new Vector3(0, 25, 0);
            RecomputePolar();
            SetCamera();
        }

        public void ResetCamera()
        {
            eye = new Vector3(70, 70, 70);
            target = new Vector3(0, 0, 0);
            up_vector = new Vector3(0, 1, 0);
            RecomputePolar();
            SetCamera();
        }
    }
}
