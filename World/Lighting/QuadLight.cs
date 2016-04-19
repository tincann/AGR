using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.World.Objects.Complex;

namespace RayTracer.World.Lighting
{
    public class QuadLight : ISurfaceLight
    {
        private readonly Quad _quad;
        public readonly Vector3 Normal;
        public Color3 Color => _quad.Material.Color;
        
        public QuadLight(Quad quad, float brightness = 1)
        {
            Brightness = brightness;
            _quad = quad;
            Normal = quad.Normal;
            Area = (quad.P2 - quad.P1).Length*(quad.P4 - quad.P1).Length;

            if (_quad.Material.MaterialType != MaterialType.Light)
            {
                _quad.Material = Material.Light;
            }
        }

        public QuadLight(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color4 color, float brightness = 1) 
            : this(new Quad(p1, p2, p3, p4, new Material(MaterialType.Light) { Color = color }))
        {
            Brightness = brightness;
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = IntersectionHelper.GetClosestIntersection(ray, _quad.Boundables);
            return intersection != null;
        }

        public SurfacePoint GetRandomPoint(RNG rng, Intersection intersection)
        {
            var u = rng.RandomFloat();
            var v = rng.RandomFloat();

            var p = _quad.P1 + u*_quad.Width + v*_quad.Depth;
            return new SurfacePoint {Location = p, Normal = _quad.Normal};
        }
        public float Area { get; private set; }
        public float Brightness { get; } = 1;
    }
}