﻿using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Intersection
    {
        public Intersection(Intersectable intersectsWith, Ray ray, Vector3 surfaceNormal, Vector3 location, float distance, Material material)
        {
            IntersectsWith = intersectsWith;
            Ray = ray;
            SurfaceNormal = surfaceNormal;
            Location = location;
            Distance = distance;
            Material = material;
        }

        public Ray Ray { get; }
        public Intersectable IntersectsWith { get; }
        public Vector3 SurfaceNormal { get; }

        public Vector3 Location { get; }
        public float Distance { get; set; }

        public Material Material { get; }
    }
}