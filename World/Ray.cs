using System.Diagnostics;
using OpenTK;
using RayTracer.Lighting;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Ray
    {

        public Ray(Vector3 origin, Vector3 direction, int bounceNumber, Intersectable originPrimitive, Material medium) : this(origin, direction, bounceNumber)
        {
            OriginPrimitive = originPrimitive;
            Medium = medium;
        }

        public Ray(Vector3 origin, Vector3 direction, int bounceNumber, Intersectable originPrimitive) : this(origin, direction, bounceNumber)
        {
            OriginPrimitive = originPrimitive;
        }

        public readonly int BounceNumber;

        public Ray(Vector3 origin, Vector3 direction, int bounceNumber)
        {
            Origin = origin;
            BounceNumber = bounceNumber + 1;
            Direction = direction.Normalized();

            //todo possible divide by zero
            InverseDirection = Vector3.Divide(Vector3.One, Direction);
        }

        public void SetLength(float length)
        {
            Debug.Assert(length <= T);
            T = length;
        }

        public Vector3 GetPoint(float t)
        {
            Debug.Assert(t >= 0);
            return Origin + t*Direction;
        }

        public static Ray Reflect(Ray ray, Intersection intersection)
        {
            var c = -Vector3.Dot(intersection.SurfaceNormal, ray.Direction);
            var reflectionDirection = ray.Direction + (2 * intersection.SurfaceNormal * c);
            return new Ray(intersection.Location, reflectionDirection, ray.BounceNumber, intersection.IntersectsWith);
        }

        public float T { get; private set; } = float.MaxValue;
        public Vector3 Origin { get; }
        public Intersectable OriginPrimitive { get; }
        public Vector3 Direction { get; }
        public Vector3 InverseDirection { get; }

        public Material Medium { get; } = Material.Air;
        
        //Is ray now inside last intersected material?
        public bool Transmitted { get; } = false;

        public static Ray CreateFromIntersection(Intersection intersection, Vector3 direction)
        {
            var epsilon = direction*0.001f;
            return new Ray(intersection.Location + epsilon, direction, intersection.Ray.BounceNumber, intersection.IntersectsWith);
        }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target, Intersectable originalPrimitive)
        {
            return new Ray(origin, target - origin, 0, originalPrimitive);
        }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target)
        {
            return new Ray(origin, target - origin, 0);
        }
    }
}
