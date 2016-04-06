using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading.Models;
using RayTracer.World;

namespace RayTracer.Shading.Tracers
{
    public class PathTracer : IRayTracer
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

            var lightingModel = new MonteCarloLightingModel(scene);
            return lightingModel.Calculate(intersection);
        }
    }
}