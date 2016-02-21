using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;

namespace RayTracer.World.Objects
{
    public abstract class Primitive : Intersectable
    {
        public MaterialType MaterialType { get; set; }
        public Color3 Color { get; set; }

        protected Primitive(MaterialType materialType, Color3 color)
        {
            MaterialType = materialType;
            Color = color;
        }

        public abstract bool Intersect(Ray ray, out Intersection intersection);
    }
}
