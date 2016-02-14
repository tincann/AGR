using OpenTK;

namespace RayTracer
{
    public class Camera
    {
        public Camera(Vector3 position, Vector3 target, float fov)
        {
            Position = position;
            Target = target;
            FOV = fov;
        }

        private Vector3 Position { get; set; }
        private Vector3 Target { get; set; }
        private float FOV { get; set; }
    }
}