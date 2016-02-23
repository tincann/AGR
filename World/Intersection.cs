﻿using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Intersection
    {
        public Intersection(Intersectable intersectsWith, Ray ray, Vector3 surfaceNormal, Vector3 location, float distance, MaterialType materialType, Color3 color)
        {
            IntersectsWith = intersectsWith;
            Ray = ray;
            SurfaceNormal = surfaceNormal;
            Location = location;
            Distance = distance;
            MaterialType = materialType;
            Color = color;
        }

        public Ray Ray { get; }
        public Intersectable IntersectsWith { get; }
        public Vector3 SurfaceNormal { get; }

        public Vector3 Location { get; }
        public float Distance { get; set; }
        public MaterialType MaterialType { get; }
        public Color3 Color { get; }
    }
}