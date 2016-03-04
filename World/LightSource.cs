using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World
{
    public class LightSource
    {
        public LightSource(Vector3 position, Color4 color, float intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }

        public Vector3 Position { get; }
        public Color4 Color { get; }
        public float Intensity { get; }
    }
}