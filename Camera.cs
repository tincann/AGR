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
        //private Vector3 _screenCenter;

        private Vector3 _position;
        private Matrix4 _cameraMatrix;

        
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
            
            //somehow it must be -d and -y
            var p0 = new Vector3(-1,  1, -d); //bottom left
            var p1 = new Vector3( 1,  1, -d); //bottom right
            var p2 = new Vector3(-1, -1, -d); //top left

            _cameraMatrix = Matrix4.LookAt(Position, Target, Vector3.UnitY);;
            _cameraMatrix.Invert();

            //Console.WriteLine($"Translation: {_cameraMatrix.ExtractTranslation()} Rotation: {_cameraMatrix.ExtractRotation()}");

            _p0     = Vector3.Transform(p0, _cameraMatrix);
            var tp1 = Vector3.Transform(p1, _cameraMatrix);
            var tp2 = Vector3.Transform(p2, _cameraMatrix);

            _e1 = tp1 - _p0;
            _e2 = tp2 - _p0;
        }

        public Ray CreatePrimaryRay(float u, float v)
        {
            var screenPoint = _p0 + u*_e1 + v*_e2;
            var direction = (screenPoint - Position).Normalized();
            return new Ray(Position, direction);
        }
    }
}