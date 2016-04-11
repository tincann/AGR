using System;
using OpenTK;
using RayTracer.Helpers;
using RayTracer.World;

namespace RayTracer.Shading.Models
{
    public class MonteCarloLightingModel
    {
        private readonly Scene _scene;

        public MonteCarloLightingModel(Scene scene)
        {
            _scene = scene;
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
                    throw new NotImplementedException();
                case MaterialType.Dielectric:
                    throw new NotImplementedException();
            }
            throw new Exception("Materialtype is not supported");
        }

        public Color3 Diffuse(Intersection intersection)
        {

            var direction = RNG.RandomVectorOnHemisphere(intersection.SurfaceNormal);
            var reflected = Ray.CreateFromIntersection(intersection, direction, goesIntoMaterial: true);

            var brdf = intersection.Material.Color/(float)Math.PI;

            var Ei = _scene.Sample(reflected) * Vector3.Dot(intersection.SurfaceNormal, direction); //irradiance
            return MathHelper.TwoPi*brdf*Ei;
        }
    }
}
