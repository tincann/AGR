using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        public LightSource LightSource { get; set; }
        public List<Primitive> Objects { get; } = new List<Primitive>();

        private static readonly Primitive Background = new Background(MaterialType.Diffuse, Color4.Black);

        public Color4 Intersect(Ray ray)
        {
            //get nearest intersection
            var intersection = GetNearestIntersection(ray);
            return intersection.Color;
        }

        private Intersection GetNearestIntersection(Ray ray)
        {
            float closestDistance = float.MaxValue;
            Primitive closestObj = Background;
            foreach (var obj in Objects)
            {
                float t;
                if (obj.Intersect(ray, out t))
                {
                    if (t < closestDistance)
                    {
                        closestDistance = t;
                        closestObj = obj;
                    }
                }
            }

            //no intersection
            if (closestObj == null)
            {
                closestObj = Background;
            }

            var intersectionPoint = ray.Direction*closestDistance;
            return new Intersection(intersectionPoint, closestObj.MaterialType, closestObj.Color);
        }
    }
}
