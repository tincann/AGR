using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;
using RayTracer.Structures;

namespace RayTracer.World.Objects
{
    public abstract class Primitive : Boundable
    {
        public Material Material { get; set; }
        public abstract BoundingBox BoundingBox { get; }
        
        protected Primitive(Material material)
        {
            Material = material;
        }

        public abstract bool Intersect(Ray ray, out Intersection intersection);
    }
}
