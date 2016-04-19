using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.World.Objects.Primitives;

namespace RayTracer.World.Lighting
{
    public class SphereLight : Sphere, ISurfaceLight
    {
        public Color3 Color => Material.Color;
        public float Area { get; }
        public float Brightness { get; } = 1;

        public SphereLight(Vector3 center, float radius, Color4 color, float brightness = 1) : base(center, radius, new Material(MaterialType.Light).WithColor(color))
        {
            Brightness = brightness;
            Area = 4*MathHelper.Pi*radius*radius / 2;
        }

        public SurfacePoint GetRandomPoint(RNG rng, Intersection intersection)
        {
            var v = rng.RandomVector().Normalized();
            var lDir = (intersection.Location - Center).Normalized();
            if (Vector3.Dot(v, lDir) > 0)
            {
                v *= -1;
            }

            return new SurfacePoint { Location = Center + v*Radius, Normal = lDir };
        }
    }
}