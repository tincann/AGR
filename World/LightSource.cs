using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;

namespace RayTracer.World
{
    public class LightSource
    {
        public LightSource(Vector3 position, Color3 color, float intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }

        public Vector3 Position { get; }
        public Color3 Color { get; }
        public float Intensity { get; }
    }
}