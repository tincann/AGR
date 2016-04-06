using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using RayTracer.World;

namespace RayTracer.Structures
{
    public class BoundingBox
    {
        public Vector3 Min, Max;
        public Vector3 Centroid { get; }
        public readonly float Area;
        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;// + new Vector3(float.Epsilon);
            Centroid = (Min + Max)/2;

            var h = Max.Y - Min.Y;
            var w = Max.X - Min.X;
            var l = Max.Z - Min.Z;
            Area = 2*(l*w + l*h + w*h);
        }

        public static BoundingBox FromVectors(params Vector3[] vectors)
        {
            var minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            
            foreach (var v in vectors)
            {
                minVector = Vector3.ComponentMin(minVector, v);
                maxVector = Vector3.ComponentMax(maxVector, v);
            }

            return new BoundingBox(minVector, maxVector);
        }

        public static BoundingBox FromBoundables(IEnumerable<Boundable> boundables)
        {
            return Combine(boundables.Select(x => x.BoundingBox).ToArray());
        }
        
        public static BoundingBox Combine(params BoundingBox[] bb)
        {
            var minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var v in bb)
            {
                minVector = Vector3.ComponentMin(minVector, v.Min);
                maxVector = Vector3.ComponentMax(maxVector, v.Max);
            }

            return new BoundingBox(minVector, maxVector);
        }

        public static BoundingBox Combine(BoundingBox b1, BoundingBox b2)
        {
            return new BoundingBox(
                Vector3.ComponentMin(b1.Min, b2.Min), 
                Vector3.ComponentMax(b1.Max, b2.Max));
        }
        
        public bool Intersect(Ray ray, out float t)
        {
            float tmin = float.NegativeInfinity, tmax = float.PositiveInfinity;

            var t1 = (Min.X - ray.Origin.X)*ray.InverseDirection.X;
            var t2 = (Max.X - ray.Origin.X)*ray.InverseDirection.X;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));

            t1 = (Min.Y - ray.Origin.Y) * ray.InverseDirection.Y;
            t2 = (Max.Y - ray.Origin.Y) * ray.InverseDirection.Y;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));

            t1 = (Min.Z - ray.Origin.Z) * ray.InverseDirection.Z;
            t2 = (Max.Z - ray.Origin.Z) * ray.InverseDirection.Z;

            tmin = Math.Max(tmin, Math.Min(t1, t2));
            tmax = Math.Min(tmax, Math.Max(t1, t2));
            t = tmin;
            return tmax >= Math.Max(tmin, 0) && ray.T >= tmin; //todo klopt dit laatste?
        }
    }
}
