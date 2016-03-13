using OpenTK;

namespace RayTracer.World
{
    public class Camera
    {
        private Vector3 _p0;
        private Vector3 _e1;
        private Vector3 _e2;
        //private Vector3 _screenCenter;

        private Matrix4 _cameraMatrix;

        private Vector3 CrossDir => -Vector3.Cross(Vector3.UnitY, ViewDirection);

        public float D = 1.5f;
        
        public Camera(Vector3 position, Vector3 target)
        {
            Update(position, target);
        }

        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }
        public Vector3 ViewDirection { get; private set; }

        private float _moveSpeed = 0.1f;

        public void Move(Vector3 direction)
        {
            var moveDir = 
                direction.X * CrossDir +
                direction.Y * Vector3.UnitY +
                direction.Z * ViewDirection;

            moveDir.Normalize();
            moveDir *= _moveSpeed;

            Update(Position + moveDir, Target + moveDir);
        }

        private float _rotSpeed = 0.05f;
        public void Rotate(Vector2 rotation)
        {
            rotation *= _rotSpeed;

            var rotMatrix = 
                Matrix4.CreateFromAxisAngle(Vector3.UnitY, -rotation.X) *
                Matrix4.CreateFromAxisAngle(CrossDir, rotation.Y);

            var viewDir = Vector3.Transform(ViewDirection, rotMatrix);
            Update(Position, Position + viewDir);
        }

        public void Update(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
            ViewDirection = (target - position).Normalized();
            Update();
        }

        public void Update(Vector3 position)
        {
            Position = position;
            Update();
        }

        public void Update()
        {
            //Console.WriteLine($"Position: {Position} Target: {Target}");
            
            //somehow it must be -d and -y
            var p0 = new Vector3(-1,  1, -D); //top left
            var p1 = new Vector3( 1,  1, -D); //top right
            var p2 = new Vector3(-1, -1, -D); //bottom left

            _cameraMatrix = Matrix4.LookAt(Position, Target, Vector3.UnitY);;
            _cameraMatrix.Invert();

            //Console.WriteLine($"Translation: {_cameraMatrix.ExtractTranslation()} Rotation: {_cameraMatrix.ExtractRotation()}");
            
            //_position = Vector3.Transform(Position, _cameraMatrix);
            _p0     = Vector3.Transform(p0, _cameraMatrix);
            var tp1 = Vector3.Transform(p1, _cameraMatrix);
            var tp2 = Vector3.Transform(p2, _cameraMatrix);

            _e1 = tp1 - _p0;
            _e2 = tp2 - _p0;
        }

        public Ray CreatePrimaryRay(float u, float v)
        {
            var screenPoint = _p0 + u*_e1 + v*_e2;
            return Ray.CreateFromTwoPoints(Position, screenPoint);
        }
    }
}