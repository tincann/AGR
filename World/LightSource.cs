using System;
using System.Collections.Generic;
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

    public interface ISurfaceLight
    {
        float Area { get; }
        Color3 Color { get; }
        Vector3 GetRandomPoint(RNG rng);

        Vector3 GetNormal(Intersection intersection);
    }

    public class SphereLight : Sphere, ISurfaceLight
    {
        public SphereLight(Vector3 center, float radius, Material material) : base(center, radius, material)
        {
            Area = (float) (4 * Math.PI * radius * radius);
        }

        public Color3 Color => Material.Color;
        public float Area { get; }
        public Vector3 GetRandomPoint(RNG rng)
        {
            var theta = rng.RandomFloat()*MathHelper.TwoPi; // 0 - 2pi
            var u = rng.RandomFloat()*2 - 1; // -1 - 1
            var v = new Vector3((float)(Math.Sqrt(1 - u * u) * Math.Cos(theta)), (float)(Math.Sqrt(1 - u * u) * Math.Sin(theta)), u);
            return v * Radius;
        }

        public Vector3 GetNormal(Intersection intersection)
        {
            throw new NotImplementedException();
        }
    }

    public class QuadLight : IMesh, ISurfaceLight
    {
        private readonly Quad _quad;
        public Color3 Color => _quad.Material.Color;
        public Vector3 GetNormal(Intersection intersection)
        {
            return _quad.Normal;
        }

        public QuadLight(Quad quad)
        {
            _quad = quad;
            Area = (quad.P2 - quad.P1).Length*(quad.P4 - quad.P1).Length;
            if (_quad.Material.MaterialType != MaterialType.Light)
            {
                _quad.Material = Material.Light;
            }
        }

        public QuadLight(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color4 color) 
            : this(new Quad(p1, p2, p3, p4, new Material(MaterialType.Light) { Color = color }))
        {
        }

        public Vector3 GetRandomPoint(RNG rng)
        {
            var u = rng.RandomFloat();
            var v = rng.RandomFloat();

            return _quad.P1 + u*_quad.Width + v*_quad.Depth;
        }

        public List<Boundable> Boundables => _quad.Boundables;
        public float Area { get; private set; }
    }
}