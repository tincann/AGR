using OpenTK;

namespace RayTracer.World
{
    public class SurfacePoint
    {
        public Vector3 Location { get; set; }
        public Vector3 Normal { get; set; }

        public SurfacePoint(Vector3 location, Vector3 normal)
        {
            Location = location;
            Normal = normal;
        }
    }
}