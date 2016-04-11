using System;
using System.Diagnostics;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading.Models;
using RayTracer.World;

namespace RayTracer.Shading.Tracers
{
    public class WhittedStyleTracer : IRayTracer
    {
        public Color3 Sample(Scene scene, Ray ray, RNG random)
        {
            //Debug.Assert(scene.BVH != null);

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

            var lightingModel = new WhittedStyleLightingModel(scene, random);
            return lightingModel.Calculate(intersection);
        }
    }
}
