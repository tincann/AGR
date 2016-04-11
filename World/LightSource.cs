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

    public class SurfaceLight : IMesh
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