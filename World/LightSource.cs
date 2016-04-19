using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Structures;
using RayTracer.World.Objects;
using RayTracer.World.Objects.Complex;
using RayTracer.World.Objects.Primitives;

namespace RayTracer.World
{
    public class PointLight
    {
        public PointLight(Vector3 position, Color3 color, float intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }

        public Vector3 Position { get; set; }
        public Color3 Color { get; }
        public float Intensity { get; }
    }

    public interface ISurfaceLight : Intersectable
    {
        RandomPoint GetRandomPoint(RNG rng, Intersection intersection);
        Color3 Color { get; }
        float Area { get; }
    }

    public class SphereLight : Sphere, ISurfaceLight
    {
        public Color3 Color => Material.Color;
        public float Area { get; }

        public SphereLight(Vector3 center, float radius, Color4 color) : base(center, radius, new Material(MaterialType.Light).WithColor(color))
        {
            Area = 4*MathHelper.Pi*radius*radius;
        }

        public RandomPoint GetRandomPoint(RNG rng, Intersection intersection)
        {
            var v = rng.RandomVector().Normalized();
            var lDir = (intersection.Location - Center).Normalized();
            if (Vector3.Dot(v, lDir) > 0)
            {
                v *= -1;
            }

            return new RandomPoint { Location = Center + v*Radius, Normal = lDir };
        }
    }

    public class RandomPoint
    {
        public Vector3 Location;
        public Vector3 Normal;
    }

    public class SurfaceLight : ISurfaceLight
    {
        private readonly Quad _quad;
        public readonly Vector3 Normal;
        public Color3 Color => _quad.Material.Color;
        
        public SurfaceLight(Quad quad)
        {
            _quad = quad;
            Normal = quad.Normal;
            Area = (quad.P2 - quad.P1).Length*(quad.P4 - quad.P1).Length;

            if (_quad.Material.MaterialType != MaterialType.Light)
            {
                _quad.Material = Material.Light;
            }
        }

        public SurfaceLight(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color4 color) 
            : this(new Quad(p1, p2, p3, p4, new Material(MaterialType.Light) { Color = color }))
        {
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = IntersectionHelper.GetClosestIntersection(ray, _quad.Boundables);
            return intersection != null;
        }

        public RandomPoint GetRandomPoint(RNG rng, Intersection intersection)
        {
            var u = rng.RandomFloat();
            var v = rng.RandomFloat();

            var p = _quad.P1 + u*_quad.Width + v*_quad.Depth;
            return new RandomPoint {Location = p, Normal = _quad.Normal};
        }
        public float Area { get; private set; }
    }
}