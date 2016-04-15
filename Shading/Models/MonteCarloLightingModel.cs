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
        private readonly bool _nee;
        private readonly bool _cosineDist;
        private readonly bool _russianRoulette;

        public MonteCarloLightingModel(Scene scene, RNG rng, bool nee, bool cosineDist, bool russianRoulette)
        {
            _scene = scene;
            _rng = rng;
            _nee = nee;
            _cosineDist = cosineDist;
            _russianRoulette = russianRoulette;
        }

        private float GetKillChance(Color3 color)
        {
            var chance = (color.R + color.G + color.B)/3;
            return MathHelper.Clamp(1 - chance, 0.1f, 0.9f);
        }

        public Color3 Calculate(Intersection intersection, bool ignoreLight)
        {
            Color3 result;
            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Light:
                    if (_nee)
                    {
                        result = ignoreLight ? Color4.Black : intersection.Material.Color;
                    }
                    else
                    {
                        result = intersection.Material.Color;
                    }
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
            float killChance = 0;
            if (_russianRoulette)
            {
                killChance = GetKillChance(intersection.Material.Color);
                if (_rng.TestChance(killChance))
                {
                    return Color4.Black;
                }
            }

            //random reflected ray
            Vector3 rDir;
            if (_cosineDist)
            {
                rDir = _rng.CosineDistributed(intersection.SurfaceNormal);
            }
            else
            {
                rDir = _rng.RandomVectorOnHemisphere(intersection.SurfaceNormal);
            }
            
            var reflected = Ray.CreateFromIntersection(intersection, rDir, goesIntoMaterial: true);

            //brdf of material
            var brdf = intersection.Material.CalculateColor(intersection)/MathHelper.Pi;

            //irradiance
            var nDotR = Vector3.Dot(intersection.SurfaceNormal, rDir);
            var Ei = _scene.Sample(reflected, _rng, true) * nDotR;

            Color3 Ld = Color4.Black;

            //use next event estimation?
            if (_nee)
            {
                if (_scene.SurfaceLights.Count > 0)
                {
                    //sample light directly - next event estimation
                    var ranLight = _scene.SurfaceLights.GetRandom(); //get random light
                    Ld = SampleLightDirectly(ranLight, brdf, intersection)*_scene.SurfaceLights.Count;
                }
            }

            Color3 result;
            if (_cosineDist)
            {
                //probability density function
                var pdf = nDotR/MathHelper.Pi;
                result = brdf*Ei/pdf + Ld;
            }
            else
            {
                result = MathHelper.TwoPi * brdf * Ei + Ld;
            }

            return result/(1 - killChance);
        }

        private Color3 SampleLightDirectly(ISurfaceLight light, Color3 brdf, Intersection intersection)
        {
            var rPoint = light.GetRandomPoint(_rng, intersection); //get random point on light
            var l = rPoint.Location - intersection.Location; //vector to light
            var dist = l.LengthFast;
            l.Normalize();
            var lightRay = Ray.CreateFromIntersection(intersection, l, dist); //ray to light
            var nlightDotL = Vector3.Dot(rPoint.Normal, -l); //light normal dot light
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
