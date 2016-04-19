using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.World.Objects;

namespace RayTracer.World.Lighting
{
    public interface ISurfaceLight : Intersectable
    {
        SurfacePoint GetRandomPoint(RNG rng, Intersection intersection);
        Color3 Color { get; }
        float Area { get; }
        float Brightness { get; }
    }
}