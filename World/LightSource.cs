using System;
using System.Collections.Generic;
using System.Linq;
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

    public interface ISurfaceLight : Boundable
    {
        float Area { get; }
        Color3 Color { get; }
        SurfacePoint GetRandomPoint(RNG rng, Vector3 orentation);
    }

    public class SphereLight : Sphere, ISurfaceLight
    {
        public SphereLight(Vector3 center, float radius, Material material) : base(center, radius, material)
        {
            Area = (float) (4 * Math.PI * radius * radius) / 2; //divided by two because only a half is visible
        }

        public Color3 Color => Material.Color;
        public float Area { get; }
        public SurfacePoint GetRandomPoint(RNG rng, Vector3 viewPoint)
        {
            throw new NotImplementedException();
            var normal = rng.RandomVectorOnHemisphere((viewPoint - Center).Normalized());    
            var location = Center + normal * Radius * 1.001f;
            return new SurfacePoint(location, normal);
        }
    }

    public class QuadLight : ISurfaceLight
    {
        private readonly Quad _quad;
        public Color3 Color => _quad.Material.Color;
        public Vector3 GetNormal(Intersection intersection)
        {
            return _quad.Normal;
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = IntersectionHelper.GetClosestIntersection(ray, Boundables);
            return intersection != null;
        }

        public BoundingBox BoundingBox { get; }

        public QuadLight(Quad quad)
        {
            _quad = quad;
            Area = (quad.P2 - quad.P1).Length*(quad.P4 - quad.P1).Length;
            BoundingBox = BoundingBox.FromBoundables(_quad.Boundables);
            if (_quad.Material.MaterialType != MaterialType.Light)
            {
                _quad.Material = Material.Light;
            }
        }

        public QuadLight(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color4 color) 
            : this(new Quad(p1, p2, p3, p4, new Material(MaterialType.Light) { Color = color }))
        {
        }

        public SurfacePoint GetRandomPoint(RNG rng, Vector3 orientation)
        {
            var u = rng.RandomFloat();
            var v = rng.RandomFloat();

            var location = _quad.P1 + u*_quad.Width + v*_quad.Depth;
            return new SurfacePoint(location, _quad.Normal);
        }

        public List<Boundable> Boundables => _quad.Boundables;
        public float Area { get; }
    }
}