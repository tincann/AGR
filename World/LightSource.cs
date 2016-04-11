using System;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
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

    public class SurfaceLight
    {
        private readonly Quad _quad;

        public SurfaceLight(Quad quad)
        {
            _quad = quad;
            if (_quad.Material.MaterialType != MaterialType.Light)
            {
                _quad.Material = Material.Light;
            }
        }

        public Vector3 GetRandomPoint()
        {
            var u = RNG.RandomFloat();
            var v = RNG.RandomFloat();

            return _quad.P1 + u*_quad.Width + v*_quad.Depth;
        }
    }
}