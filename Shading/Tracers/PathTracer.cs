using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Shading.Tracers
{
    class PathTracer : IRayTracer
    {
        public Color3 Sample(Scene scene, Ray ray)
        {
            if (ray.BouncesLeft < 1)
            {
                return Color4.Red;
            }

            //get nearest intersection
            var intersection = IntersectionHelper.GetClosestIntersection(ray, scene.Objects);
            if (intersection == null)
            {
                return scene.Skybox.Intersect(ray.Direction);
            }

            return Color4.Black;
        }
    }
}