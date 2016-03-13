using OpenTK;
using RayTracer.Shading;

namespace RayTracer.World.Objects
{
    public class Plane : Primitive
    {
        private readonly Vector3 _normal;
        private readonly float _height;

        public Plane(Vector3 normal, float height, Material material) : base(material)
        {
            _normal = normal;
            _height = height;
        }
        
        public override bool Intersect(Ray ray, out Intersection intersection)
        {
            var t = -(Vector3.Dot(ray.Origin, _normal) + _height)/Vector3.Dot(ray.Direction, _normal);
            if (t >= 0)
            {
                var loc = ray.GetPoint(t);
                intersection = new Intersection(this, ray, _normal, loc, t, Material);
                return true;
            }

            intersection = null;
            return false;
        }
    }
}
