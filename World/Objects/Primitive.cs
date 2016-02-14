using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World.Objects
{
    public abstract class Primitive : Intersectable
    {
        public MaterialType MaterialType { get; set; }
        public Color4 Color { get; set; }

        protected Primitive(MaterialType materialType, Color4 color)
        {
            MaterialType = materialType;
            Color = color;
        }

        public abstract bool Intersect(Ray ray, out Intersection intersection);
    }
}
