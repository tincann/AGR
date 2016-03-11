using System;
using OpenTK;
using OpenTK.Graphics;
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

        public static Color3 Specular(Scene scene, Intersection intersection)
        {
            var mat = intersection.Material;
            var reflectedRay = Ray.Reflect(intersection.Ray, intersection);
            return mat.Specularity * scene.Intersect(reflectedRay) + DirectIllumination(scene, intersection) * (1 - mat.Specularity);
        }

        public static Color3 Dielectric(Scene scene, Intersection intersection)
        {
            var n1 = 1;
            var n2 = intersection.Material.RefractiveIndex;
            float n = n1/n2;

            var ray = intersection.Ray;
            var normal = intersection.SurfaceNormal;
            //if (inside)
            //{
            //    normal *= -1;
            //}

            float cost = Vector3.Dot(normal, -ray.Direction);
            float k = 1 - n*n*(1 - cost*cost);
            if (k < 0)
            {
                //internal reflection
                return new Color3(Color4.Green);
            }

            var T = n*ray.Direction - normal*(n*cost + (float)Math.Sqrt(k));
            var eps = T*0.0001f;
            var refracted = new Ray(intersection.Location + eps, T, ray.BounceNumber, intersection.IntersectsWith);
            float R0 = (n1 - n2)/(n1 + n2);
            R0 *= R0;
            var a = 1 - cost;
            float Fr = R0 + (1 - R0)*a*a*a*a*a;
            float Ft = 1 - Fr;


            //Intersection internalIntersection;
            //intersection.IntersectsWith.Intersect(refracted, out internalIntersection);


            return Ft * scene.Intersect(refracted) + Fr * Specular(scene, intersection);
            //var normal = inside ? -intersection.SurfaceNormal : intersection.SurfaceNormal;
            //float cosI = Vector3.Dot(normal, intersection.Ray.Direction);
            //float sinT2 = n*n*(1 - cosI*cosI);
            //if (sinT2 > 1)
            //{
            //    //no refraction, only reflection
            //    return new Color3(Color4.Green);//Specular(scene, intersection);
            //}

            //var refractedVector = n*intersection.Ray.Direction - (n + (float)Math.Sqrt(1 - sinT2))*normal;
            //var refractedRay = new Ray(intersection.Location, refractedVector, intersection.Ray.BounceNumber, intersection.IntersectsWith, true);

            ////reflection percentage
            //var r02 = (n1 - n2)/(n1 + n2);
            //var r0 = r02*r02;

            //var a = (1 - Vector3.Dot(intersection.SurfaceNormal, intersection.Ray.Direction));
            //var fr = r0 + (1 - r0)*a*a*a*a*a; //reflection percentage
            //var ft = 1 - fr; //refraction precentage

            //return scene.Intersect(refractedRay);
            //return fr*Specular(scene, intersection) + ft* scene.Intersect(refractedRay);
        }
    }
}
