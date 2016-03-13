﻿using System;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Lighting
{
    public static class LightingModel
    {
        public static Color3 DirectIllumination(Scene scene, Intersection intersection)
        {
            var color = new Color3(Color4.Black);
            foreach (var light in scene.LightSources)
            {
                var lightVector = light.Position - intersection.Location;
                var invLightDistance2 = 1/lightVector.LengthSquared;
                var shadowRay = Ray.CreateFromIntersection(intersection, lightVector);
                if (IntersectionHelper.DoesIntersect(shadowRay, scene.Objects))
                {
                    continue;
                }

                var intensity = Vector3.Dot(intersection.SurfaceNormal, shadowRay.Direction);
                color += Math.Max(intensity, 0) * intersection.Material.Color * invLightDistance2*light.Intensity;
            }

            return color;
        }

        public static Color3 Specular(Scene scene, Intersection intersection)
        {
            var mat = intersection.Material;
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);

            var specColor = new Color3(Color4.Black);
            foreach (var lightSource in scene.LightSources)
            {
                var lightDir = (lightSource.Position - intersection.Location).Normalized();
                var h = (-intersection.Ray.Direction + lightDir).Normalized();
                var dot = Vector3.Dot(h, intersection.SurfaceNormal);
                if (dot > 0)
                {
                    var spec = (float)Math.Pow(dot, 32) * intersection.Material.Specularity;
                    specColor += lightSource.Color * spec;
                }
            }

            return mat.Specularity * (specColor + scene.Intersect(reflectedRay)) + (1 - mat.Specularity) * DirectIllumination(scene, intersection);
        }

        public static Color3 Dielectric(Scene scene, Intersection intersection)
        {
            var n1 = intersection.Ray.Medium.RefractiveIndex;
            var n2 = intersection.Material.RefractiveIndex;
            float n = n1/n2;

            var ray = intersection.Ray;
            var normal = intersection.SurfaceNormal;
            
            float cost = Vector3.Dot(normal, -ray.Direction);
            float k = 1 - n*n*(1 - cost*cost);
            if (k < 0)
            {
                //internal reflection
                return Specular(scene, intersection);
            }

            var T = n*ray.Direction + normal*(n*cost - (float) Math.Sqrt(k));

            var epsilon = T * 0.0001f;
            var refracted = new Ray(intersection.Location + epsilon, T, ray.BouncesLeft, intersection.IntersectsWith, intersection.Material);
            float R0 = (n1 - n2)/(n1 + n2);
            R0 *= R0;
            var a = 1 - cost;
            float Fr = R0 + (1 - R0)*a*a*a*a*a;
            float Ft = 1 - Fr;

            return Ft * scene.Intersect(refracted) + Fr * Specular(scene, intersection);
        }
    }
}
