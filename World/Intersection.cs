using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World
{
    public class Intersection
    {
        public Intersection(Vector3 location, MaterialType materialType, Color4 color)
        {
            Location = location;
            MaterialType = materialType;
            Color = color;
        }

        public Vector3 Location { get; }
        public MaterialType MaterialType { get; }
        public Color4 Color { get; }
    }
}