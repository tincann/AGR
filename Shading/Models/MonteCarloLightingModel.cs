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

        public Color3 Calculate(Intersection intersection, bool ignoreLight)
        {
            //if (_rng.TestChance(Constants.RussianRouletteDieChance))
            //{
            //    return Color4.Black;
            //}

            Color3 result;
            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Light:
                    //result = ignoreLight ? Color4.Black : intersection.Material.Color;
                    result = intersection.Material.Color;
                    break;
                case MaterialType.Diffuse:
                    result = Diffuse(intersection);
                    break;
                case MaterialType.Specular:
                    result = Specular(intersection);
                    break;
                case MaterialType.Dielectric:
                    result = Dielectric(intersection);
                    break;
                default:
                    throw new Exception("Materialtype is not supported");
            }

            //return result / (1 - Constants.RussianRouletteDieChance);
            return result;
        }

        public Color3 Diffuse(Intersection intersection)
        {
            //random reflected ray
            var rDir = _rng.CosineDistributed(intersection.SurfaceNormal);
            var reflected = Ray.CreateFromIntersection(intersection, rDir, goesIntoMaterial: true);

            //brdf of material
            var brdf = intersection.Material.CalculateColor(intersection)/(float)Math.PI;

            Color3 Ld = Color4.Black;
            //if (_scene.SurfaceLights.Count > 0)
            //{
            //    //sample light directly - next event estimation
            //    var ranLight = _scene.SurfaceLights.GetRandom(); //get random light
            //    Ld = SampleLightDirectly(ranLight, brdf, intersection) * _scene.SurfaceLights.Count;
            //}

            //irradiance
            var nDotR = Vector3.Dot(intersection.SurfaceNormal, rDir);
            var Ei = _scene.Sample(reflected, _rng, true) * nDotR;
            
            //probability density function
            //var pdf = nDotR / MathHelper.Pi;
            var pdf = 1;

            return brdf*Ei / pdf + Ld;
        }

        private Color3 SampleLightDirectly(SurfaceLight light, Color3 brdf, Intersection intersection)
        {
            var lPoint = light.GetRandomPoint(_rng, intersection); //get random point on light
            var l = lPoint - intersection.Location; //vector to light
            var dist = l.LengthFast;
            var lightRay = Ray.CreateFromIntersection(intersection, l, dist - 0.01f); //ray to light - epsilon to not intersect with light itself
            var nlightDotL = Vector3.Dot(light.Normal, -l); //light normal dot light
            var nDotL = Vector3.Dot(intersection.SurfaceNormal, l); //normal dot light

            if (nDotL > 0 && nlightDotL > 0 && !IntersectionHelper.DoesIntersect(lightRay, _scene.Objects, light))
            {
                var solidAngle = (nlightDotL * light.Area) / (dist * dist); //light area on hemisphere
                return light.Color * solidAngle * brdf * nDotL;
            }

            return Color4.Black;
        }
        
        public Color3 Specular(Intersection intersection)
        {
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return intersection.Material.CalculateColor(intersection) * _scene.Sample(reflectedRay, _rng, false);
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
                return transparency*_scene.Sample(refracted, _rng, false);
            }

            //reflect outside
            return transparency*Specular(intersection);
        }
    }
}
