using RayTracer.Lighting;

namespace RayTracer.World.Objects
{
    public abstract class Primitive : Intersectable
    {
        public Material Material { get; set; }
        
        protected Primitive(Material material)
        {
            Material = material;
        }

        public abstract bool Intersect(Ray ray, out Intersection intersection);
    }
}
