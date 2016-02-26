using System;
using OpenTK;
using RayTracer.Lighting;
using RayTracer.Structures;

namespace RayTracer.World.Objects
{
    public class Sphere : Primitive
    {
        public Vector3 Center { get; }
        public float Radius { get; }
        public override BoundingBox BoundingBox { get; }
        private readonly float _rad2;
        public Sphere(Vector3 center, float radius, Material material) : base(material)
        {
            Center = center;
            Radius = radius;
            _rad2 = radius*radius;
            var radVector = new Vector3(radius, radius, radius);
            BoundingBox = new BoundingBox(Center - radVector, Center + radVector);
        }

        public override bool Intersect(Ray ray, out Intersection intersection)
        {
            //todo check if inside sphere
            intersection = null;

            var c = Center - ray.Origin;
            float t = Vector3.Dot(c, ray.Direction);
            var q = c - t*ray.Direction;
            float p2 = Vector3.Dot(q, q);
            if (p2 > _rad2) return false;
            t -= (float)Math.Sqrt(_rad2 - p2);
            if (t > 0)
            {
                var intersectionPoint = ray.GetPoint(t);
                var normal = (intersectionPoint - Center).Normalized();
                intersection = new Intersection(this, ray, normal, intersectionPoint, t, Material);
                return true;
            }

            return false;
        }
    }
}
