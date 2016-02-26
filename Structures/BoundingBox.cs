using System;
using OpenTK;
using RayTracer.World;

namespace RayTracer.Structures
{
    public class BoundingBox
    {
        private Vector3 _min, _max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            _min = min;
            _max = max;
        }

        public static BoundingBox Combine(BoundingBox b1, BoundingBox b2)
        {
            return new BoundingBox(
                Vector3.ComponentMin(b1._min, b2._min), 
                Vector3.ComponentMax(b1._max, b2._max));
        }

        public bool Intersect(Ray ray)
        {
            float tmin = float.NegativeInfinity, tmax = float.PositiveInfinity;

            var t1 = (_min.X - ray.Origin.X)*ray.InverseDirection.X;
            var t2 = (_max.X - ray.Origin.X)*ray.InverseDirection.X;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));

            t1 = (_min.Y - ray.Origin.Y) * ray.InverseDirection.Y;
            t2 = (_max.Y - ray.Origin.Y) * ray.InverseDirection.Y;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));

            t1 = (_min.Z - ray.Origin.Z) * ray.InverseDirection.Z;
            t2 = (_max.Z - ray.Origin.Z) * ray.InverseDirection.Z;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));

            return tmax > Math.Max(tmin, 0);
        }
    }
}
