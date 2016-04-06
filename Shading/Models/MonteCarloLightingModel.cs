using System;
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
                    throw new NotImplementedException();
                case MaterialType.Specular:
                    throw new NotImplementedException();
                case MaterialType.Dielectric:
                    throw new NotImplementedException();
            }
            throw new Exception("Materialtype is not supported");
        }

        public Color3 Diffuse(Intersection intersection)
        {
            // construct vector to random point on light
            //var randomLightPoints = 
            //var L = Scene.RandomPointOnLight() - I;
            //float dist = L.Length();
            //L /= dist;
            //float cos_o = Vector3.Dot(-L, new Vector3(0, -1, 0));
            //float cos_i = Vector3.Dot(L, ray.N);
            //if ((cos_o <= 0) || (cos_i <= 0)) return BLACK;
            //// light is not behind surface point, trace shadow ray
            //Ray r = new Ray(I + EPSILON * L, L, dist - 2 * EPSILON);
            //Scene.Intersect(r);
            //if (r.objIdx != -1) return Vector3.Zero;
            //// light is visible (V(p,p’)=1); calculate transport
            //Vector3 BRDF = material.diffuse * INVPI;
            //float solidAngle = (cos_o * Scene.LIGHTAREA) / (dist * dist);
            //return BRDF * Scene.lightColor * solidAngle * cos_i;
            throw new NotImplementedException();
        }
    }
}
