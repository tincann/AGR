using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World
{
    public class Intersection
    {
        public Intersection(Vector3 location, float distance, MaterialType materialType, Color4 color)
        {
            Location = location;
            Distance = distance;
            MaterialType = materialType;
            Color = color;
        }

        public Vector3 Location { get; }
        public float Distance { get; set; }
        public MaterialType MaterialType { get; }
        public Color4 Color { get; }
    }
}