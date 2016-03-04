using OpenTK;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Ray
    {
        public Ray(Vector3 origin, Vector3 direction, Intersectable originPrimitive) : this(origin, direction)
        {
            OriginPrimitive = originPrimitive;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction.Normalized();

            //todo possible divide by zero
            InverseDirection = Vector3.Divide(Vector3.One, Direction);
        }

        public void SetLength(float length)
        {
            T = length;
        }

        public Vector3 GetPoint(float t)
        {
            return Origin + t*Direction;
        }

        public static Ray Reflect(Ray ray, Intersection intersection)
        {
            var c = -Vector3.Dot(intersection.SurfaceNormal, ray.Direction);
            var reflectionDirection = ray.Direction + (2 * intersection.SurfaceNormal * c);
            return new Ray(intersection.Location, reflectionDirection, intersection.IntersectsWith);
        }

        public float T { get; private set; } = float.MaxValue;
        public Vector3 Origin { get; }
        public Intersectable OriginPrimitive { get; }
        public Vector3 Direction { get; }
        public Vector3 InverseDirection { get; }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target, Intersectable originalPrimitive)
        {
            return new Ray(origin, target - origin, originalPrimitive);
        }

        public static Ray CreateFromTwoPoints(Vector3 origin, Vector3 target)
        {
            return new Ray(origin, target - origin);
        }
    }
}
