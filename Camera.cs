using OpenTK;
using RayTracer.World;

namespace RayTracer
{
    public class Camera
    {
        private Vector3 _p0;
        private Vector3 _e1;
        private Vector3 _e2;

        private Vector3 _screenCenter;
        private readonly float d = 1;
        private float FOV;

        public Camera(Vector3 position, Vector3 direction, float fov)
        {
            Position = position;
            Direction = direction;
            FOV = fov;

            Update();
        }

        public Vector3 Position { get; }
        public Vector3 Direction { get; }

        public void Update()
        {
            _screenCenter = Position + d*Direction;
            _p0 = _screenCenter + new Vector3(-1, -1, 0);
            var p1 = _screenCenter + new Vector3(1, -1, 0);
            var p2 = _screenCenter + new Vector3(-1, 1, 0);

            _e1 = p1 - _p0;
            _e2 = p2 - _p0;
        }

        public Ray GetRay(float u, float v)
        {
            var p = _p0 + u*_e1 + v*_e2;
            p.Normalize();
            return new Ray(Position, p);
        }
    }
}