using System;
using System.Diagnostics;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Shading.Tracers
{
    public class WhittedStyleTracer : IRayTracer
    {

        public Color3 Sample(Scene scene, Ray ray)
        {
            Debug.Assert(scene.BVH != null);

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

            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Diffuse:
                    return LightingModel.DirectIllumination(scene, intersection);
                case MaterialType.Specular:
                    return LightingModel.Specular(scene, intersection);
                case MaterialType.Dielectric:
                    return LightingModel.Dielectric(scene, intersection);
            }
            throw new NotImplementedException();
        }
    }
}
