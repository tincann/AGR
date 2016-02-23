using System;
using OpenTK;
using RayTracer.World;

namespace RayTracer.Lighting
{
    public static class LightingModel
    {
        public static Color3 DirectIlumination(Scene scene, Intersection intersection, LightSource light)
        {
            var color = intersection.Color;
            var shadowRay = Ray.CreateFromTwoPoints(intersection.Location, light.Position);
            if (scene.DoesIntersect(shadowRay))
            {
                return 0.1f*color;
            }

            var intensity = Vector3.Dot(intersection.SurfaceNormal, shadowRay.Direction);
            return Math.Abs(intensity)*color;
        }

        public static Color3 Reflection(Scene scene, Intersection intersection, float reflectionRatio)
        {
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return reflectionRatio * scene.Intersect(reflectedRay) + intersection.Color * (1 - reflectionRatio);
        }
    }
}
