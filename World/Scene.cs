using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        public LightSource LightSource { get; set; }
        public List<Intersectable> Objects { get; } = new List<Intersectable>();

        private static readonly Intersection Background = new Intersection(Vector3.Zero, float.MaxValue, MaterialType.Diffuse, Color4.Black);

        public Color4 Intersect(Ray ray)
        {
            //get nearest intersection
            var intersection = GetNearestIntersection(ray);
            return intersection.Color;
        }

        private Intersection GetNearestIntersection(Ray ray)
        {
            float closestDistance = float.MaxValue;
            var closestIntersection = Background;
            foreach (var obj in Objects)
            {
                Intersection intersection;
                if (obj.Intersect(ray, out intersection))
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
    }
}
