using System.Diagnostics;
using OpenTK;
using RayTracer.Shading;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Ray
    {

        public Ray(Vector3 origin, Vector3 direction, int bouncesLeft, Intersectable originPrimitive, Material medium) : this(origin, direction, bouncesLeft)
        {
            OriginPrimitive = originPrimitive;
            Medium = medium;
        }

        public Ray(Vector3 origin, Vector3 direction, int bouncesLeft, Intersectable originPrimitive) : this(origin, direction, bouncesLeft)
        {
            OriginPrimitive = originPrimitive;
        }

        public readonly int BouncesLeft;

        public Ray(Vector3 origin, Vector3 direction, int bouncesLeft)
        {
            Origin = origin;
            BouncesLeft = bouncesLeft - 1;
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
            return new Ray(intersection.Location, reflectionDirection, ray.BouncesLeft, intersection.IntersectsWith);
        }

        public float T { get; private set; } = float.MaxValue;
        public Vector3 Origin { get; }
        public Intersectable OriginPrimitive { get; }
        public Vector3 Direction { get; }
        public Vector3 InverseDirection { get; }

        public Material Medium { get; } = Material.Air;
        
        //Is ray now inside last intersected material?
        public bool Transmitted { get; } = false;

        public static Ray CreateFromIntersection(Intersection intersection, Vector3 direction, bool goesIntoMaterial = false)
        {
            var epsilon = direction.Normalized()*0.001f;
            var medium = goesIntoMaterial ? intersection.Material : intersection.Ray.Medium;
            return new Ray(intersection.Location + epsilon, direction, intersection.Ray.BouncesLeft, intersection.IntersectsWith, medium);
        }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target, Intersectable originalPrimitive)
        {
            return new Ray(origin, target - origin, Constants.MaxRayBounces, originalPrimitive);
        }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target)
        {
            return new Ray(origin, target - origin, Constants.MaxRayBounces);
        }
    }
}
