using OpenTK;

namespace RayTracer.World
{
    public class LightSource
    {
        public LightSource(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; set; }
    }
}