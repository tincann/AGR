using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World
{
    public class LightSource
    {
        public LightSource(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; }
        public Color4 Color { get; }
    }
}