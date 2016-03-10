using System;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.World;

namespace RayTracer.Lighting
{
    public static class LightingModel
    {
        public static Color3 DirectIlumination(Scene scene, Intersection intersection)
        {
            var color = new Color3(Color4.Black);
            foreach (var light in scene.LightSources)
            {
                var lightVector = light.Position - intersection.Location;
                var invLightDistance2 = 1/lightVector.LengthSquared;
                var shadowRay = new Ray(intersection.Location + Constants.ShadowRayEpsilon * intersection.SurfaceNormal, lightVector, intersection.Ray.BounceNumber);
                if (scene.DoesIntersect(shadowRay))
                {
                    continue;
                }

                var intensity = Vector3.Dot(intersection.SurfaceNormal, shadowRay.Direction);
                color += Math.Abs(intensity) * intersection.Material.Color * invLightDistance2*light.Intensity;
            }

            return color;
        }

        public static Color3 Reflection(Scene scene, Intersection intersection)
        {
            var mat = intersection.Material;
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return mat.ReflectionPercentage * scene.Intersect(reflectedRay) + DirectIlumination(scene, intersection) * (1 - mat.ReflectionPercentage);
        }
    }
}
