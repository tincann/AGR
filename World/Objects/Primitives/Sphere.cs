using System;
using OpenTK;
using RayTracer.Shading;
using RayTracer.Structures;

namespace RayTracer.World.Objects.Primitives
{
    public class DebugSphere : Sphere
    {
        private readonly PointLight _light;

        public DebugSphere(PointLight light, float radius, Material material) : base(light.Position, radius, material)
        {
            _light = light;
        }

        public override Vector3 Center => _light.Position;
    }

    public class Sphere : Primitive, Boundable
    {
        public virtual Vector3 Center { get; }
        public float Radius { get; }
        public BoundingBox BoundingBox { get; }
        private readonly float _rad2;
        public Sphere(Vector3 center, float radius, Material material) : base(material)
        {
            Center = center;
            Radius = radius;
            _rad2 = radius*radius;
            var radVector = new Vector3(radius);
            BoundingBox = new BoundingBox(center - radVector, center + radVector);
        }

        public static Sphere CreateOnGround(Vector3 groundPoint, float radius, Material material)
        {
            var center = groundPoint + new Vector3(0, radius, 0);
            return new Sphere(center, radius, material);
        }

        public override bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = null;

            var C = ray.Origin - Center;
            float t;
            //inside sphere
            var dotC = Vector3.Dot(C, C);
            var insideSphere = dotC <= _rad2;
            if (insideSphere)
            {
                //slow way to calculate
                var a = Vector3.Dot(ray.Direction, ray.Direction);
                var b = Vector3.Dot(2*ray.Direction, C);
                var c = dotC - _rad2;
                var d = b*b - 4*a*c;

                //no intersection
                if (d < 0)
                {
                    return false;
                }

                var sd = (float)Math.Sqrt(d);
                var t1 = (-b + sd)/2*a;
                var t2 = (-b - sd)/2*a;
                t = Math.Max(t1, t2);
            }
            else
            {
                //fast way to calculate
                t = Vector3.Dot(-C, ray.Direction);
                var q = -C - t*ray.Direction;
                float p2 = Vector3.Dot(q, q);
                if (p2 > _rad2) return false;
                t -= (float) Math.Sqrt(_rad2 - p2);
            }

            if (t > float.Epsilon)
            {
                var intersectionPoint = ray.GetPoint(t);
                var normal = (intersectionPoint - Center).Normalized();
                if (insideSphere)
                {
                    normal *= -1;
                }
                var mat = insideSphere ? Material.Air : Material;
                intersection = new Intersection(this, ray, normal, intersectionPoint, t, mat, insideSphere);
                return true;
            }

            return false;
        }
    }
}
