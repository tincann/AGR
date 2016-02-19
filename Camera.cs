using OpenTK;
using RayTracer.World;
using System;

namespace RayTracer
{
    public class Camera
    {
        private Vector3 _p0;
        private Vector3 _e1;
        private Vector3 _e2;

        private Matrix4 _cameraMatrix;

        private Vector3 _screenCenter;
        public float d = 1;
        private float FOV;

        public Camera(Vector3 position, Vector3 target, float fov)
        {
            Position = position;
            Target = target;
            FOV = fov;
            Update(position, target);
        }

        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }

        public void Update(Vector3 position, Vector3 target)
        {
            Position = Position;
            Target = target;
            Update();
        }

        public void Update(Vector3 position)
        {
            Position = position;
            Update();
        }

        public void Update()
        {
            Console.WriteLine($"Position: {Position} Target: {Target}");
            _cameraMatrix = Matrix4.LookAt(Position, Target, Vector3.UnitY);

            var direction = Target.Normalized();
            _screenCenter = Position + d * direction;

            var p0 = _screenCenter + new Vector3(-1, -1, 0);
            var p1 = _screenCenter + new Vector3(1, -1, 0);
            var p2 = _screenCenter + new Vector3(-1, 1, 0);
            
            _p0 = Vector3.Transform(p0, _cameraMatrix);

            var e1 = p1 - p0;
            var e2 = p2 - p0;

            _e1 = -Vector3.Transform(e1, _cameraMatrix);
            _e2 = Vector3.Transform(e2, _cameraMatrix);
        }

        public Ray CreatePrimaryRay(float u, float v)
        {
            var p = _p0 + u*_e1 + v*_e2;
            p.Normalize();
            return new Ray(Position, p);
        }
    }
}