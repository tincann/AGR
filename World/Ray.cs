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
        }

        public Vector3 Origin { get; }
        public Intersectable OriginPrimitive { get; }
        public Vector3 Direction { get; }

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
