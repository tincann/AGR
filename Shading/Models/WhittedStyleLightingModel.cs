using System;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Shading.Models
{
    public class WhittedStyleLightingModel
    {
        private readonly Scene _scene;

        public WhittedStyleLightingModel(Scene scene)
        {
            _scene = scene;
        }

        public Color3 Calculate(Intersection intersection)
        {
            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Light:
                case MaterialType.Diffuse:
                    return DirectIllumination(intersection);
                case MaterialType.Specular:
                    return Specular(intersection);
                case MaterialType.Dielectric:
                    return Dielectric(intersection);
            }

            throw new Exception("Materialtype is not supported");
        }

        public Color3 DirectIllumination(Intersection intersection)
        {
            var totalIntensity = 0.0f;
            foreach (var light in _scene.PointLights)
            {
                var lightVector = light.Position - intersection.Location;
                var invLightDistance2 = 1/lightVector.LengthSquared;
                var shadowRay = Ray.CreateFromIntersection(intersection, lightVector);
                if (IntersectionHelper.DoesIntersect(shadowRay, _scene.Objects))
                {
                    continue;
                }

                var intensity = Vector3.Dot(intersection.SurfaceNormal, shadowRay.Direction);
                totalIntensity += Math.Max(intensity, 0) * invLightDistance2*light.Intensity;
            }
            var color = intersection.Material.Texture == null
                    ? intersection.Material.Color
                    : intersection.Material.Texture.GetColor(intersection.Location);
            return totalIntensity * color;
        }

        public Color3 Specular(Intersection intersection)
        {
            var mat = intersection.Material;
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);

            Color3 specColor = Color4.Black;
            foreach (var lightSource in _scene.PointLights)
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

            return mat.Specularity * (specColor + _scene.Sample(reflectedRay)) + (1 - mat.Specularity) * DirectIllumination(intersection);
        }

        public Color3 Dielectric(Intersection intersection)
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
                return Specular(intersection);
            }

            var T = n*ray.Direction + normal*(n*cost - (float) Math.Sqrt(k));
            
            var refracted = Ray.CreateFromIntersection(intersection, T, true);

            float R0 = (n1 - n2)/(n1 + n2);
            R0 *= R0;
            var a = 1 - cost;
            float Fr = R0 + (1 - R0)*a*a*a*a*a;
            float Ft = 1 - Fr;

            var transparency = new Vector3(1);
            if (intersection.InsidePrimitive)
            {
                var absorbance = intersection.Ray.Medium.Color * intersection.Ray.Medium.Absorbance * -intersection.Distance;
                transparency = new Vector3(
                    (float)Math.Exp(-absorbance.R),
                    (float)Math.Exp(-absorbance.G),
                    (float)Math.Exp(-absorbance.B)); 
            }
            var color = Ft* _scene.Sample(refracted) + Fr * Specular(intersection);
            return transparency*color;
        }
    }
}
