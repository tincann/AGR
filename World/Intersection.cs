using OpenTK;
using RayTracer.Shading;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Intersection
    {
        public Intersection(Intersectable intersectsWith, Ray ray, Vector3 surfaceNormal, Vector3 location,
            float distance, Material material, bool insidePrimitive)
            : this(intersectsWith, ray, surfaceNormal, location, distance, material)
        {
            InsidePrimitive = insidePrimitive;
        }

        public Intersection(Intersectable intersectsWith, Ray ray, Vector3 surfaceNormal, Vector3 location, float distance, Material material)
        {
            IntersectsWith = intersectsWith;
            Ray = ray;
            SurfaceNormal = surfaceNormal;
            Location = location;
            Distance = distance;
            Material = material;
        }

        public Ray Ray { get; }
        public Intersectable IntersectsWith { get; }
        public Vector3 SurfaceNormal { get; }

        public Vector3 Location { get; }
        public float Distance { get; set; }

        public Material Material { get; }
        public bool InsidePrimitive { get; set; }
    }
}