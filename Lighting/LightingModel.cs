using System;
using OpenTK;
using RayTracer.World;

namespace RayTracer.Lighting
{
    public static class LightingModel
    {
        public static Color3 DirectIlumination(Scene scene, Intersection intersection, LightSource light)
        {
            var color = intersection.Material.Color;
            var lightVector = light.Position - intersection.Location;
            var invLightDistance2 = 1/lightVector.LengthSquared;
            var shadowRay = new Ray(intersection.Location, lightVector);
            if (scene.DoesIntersect(shadowRay))
            {
                return 0.1f*color;
            }

            var intensity = Vector3.Dot(intersection.SurfaceNormal, shadowRay.Direction);
            return Math.Abs(intensity) * color * invLightDistance2 * light.Intensity;
        }

        public static Color3 Reflection(Scene scene, Intersection intersection, LightSource light)
        {
            var mat = intersection.Material;
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return mat.ReflectionPercentage * scene.Intersect(reflectedRay) + DirectIlumination(scene, intersection, light) * (1 - mat.ReflectionPercentage);
        }
    }
}
