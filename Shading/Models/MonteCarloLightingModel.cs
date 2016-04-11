using System;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Shading.Models
{
    public class MonteCarloLightingModel
    {
        private readonly Scene _scene;
        private readonly RNG _rng;

        public MonteCarloLightingModel(Scene scene, RNG rng)
        {
            _scene = scene;
            _rng = rng;
        }

        public Color3 Calculate(Intersection intersection)
        {
            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Light:
                    return intersection.Material.Color;
                case MaterialType.Diffuse:
                    return Diffuse(intersection);
                case MaterialType.Specular:
                    return Specular(intersection);
                case MaterialType.Dielectric:
                    return Dielectric(intersection);
            }
            throw new Exception("Materialtype is not supported");
        }

        public Color3 Diffuse(Intersection intersection)
        {

            var direction = _rng.RandomVectorOnHemisphere(intersection.SurfaceNormal);
            var reflected = Ray.CreateFromIntersection(intersection, direction, goesIntoMaterial: true);

            var brdf = intersection.Material.CalculateColor(intersection)/(float)Math.PI;

            var Ei = _scene.Sample(reflected, _rng) * Vector3.Dot(intersection.SurfaceNormal, direction); //irradiance
            return MathHelper.TwoPi*brdf*Ei;
        }


        public Color3 Specular(Intersection intersection)
        {
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return intersection.Material.CalculateColor(intersection) * _scene.Sample(reflectedRay, _rng);
        }

        public Color3 Dielectric(Intersection intersection)
        {
            var n1 = intersection.Ray.Medium.RefractiveIndex;
            var n2 = intersection.Material.RefractiveIndex;
            float n = n1 / n2;

            var ray = intersection.Ray;
            var normal = intersection.SurfaceNormal;

            float cost = Vector3.Dot(normal, -ray.Direction);
            float k = 1 - n * n * (1 - cost * cost);
            if (k < 0)
            {
                //internal reflection
                return Specular(intersection);
            }

            var T = n * ray.Direction + normal * (n * cost - (float)Math.Sqrt(k));

            var refracted = Ray.CreateFromIntersection(intersection, T, float.MaxValue, true);

            float R0 = (n1 - n2) / (n1 + n2);
            R0 *= R0;
            var a = 1 - cost;
            float Fr = R0 + (1 - R0) * a * a * a * a * a;
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

            //go inside
            if (_rng.RandomFloat() < Ft)
            {
                return transparency*_scene.Sample(refracted, _rng);
            }

            //reflect outside
            return transparency*Specular(intersection);
        }
    }
}
