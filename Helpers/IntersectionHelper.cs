using System;
using System.Collections.Generic;
using RayTracer.World;
using RayTracer.World.Objects;
using RayTracer.World.Objects.Primitives;

namespace RayTracer.Helpers
{
    public static class IntersectionHelper
    {
        public static Intersection GetClosestIntersection(Ray ray, IEnumerable<Intersectable> intersectables)
        {
            float closestDistance = float.MaxValue;
            Intersection closestIntersection = null;
            foreach (var obj in intersectables)
            {
                Intersection intersection;
                if (obj.Intersect(ray, out intersection))// && !ReferenceEquals(ray.OriginPrimitive, obj))
                {
                    if (intersection.Distance < closestDistance)
                    {
                        closestDistance = intersection.Distance;
                        closestIntersection = intersection;
                    }
                }
            }

            return closestIntersection;
        }

        
        public static bool DoesIntersect(Ray ray, IEnumerable<Intersectable> intersectables, Intersectable ignore = null)
        {
            foreach (var obj in intersectables)
            {
                //if (obj is DebugSphere)
                //{
                //    continue;
                //}
                Intersection intersection;
                if (obj != ignore && obj.Intersect(ray, out intersection))// && !ReferenceEquals(ray.OriginPrimitive, obj))
                {
                    return true;
                }
            }
            return false;
        }

        public static Intersection GetMinimumIntersection(Intersection i1, Intersection i2)
        {
            if (i1 == null)
            {
                return i2;
            }

            if (i2 == null)
            {
                return i1;
            }

            if (i1.Distance < i2.Distance)
            {
                return i1;
            }
            return i2;
        }
    }
}
