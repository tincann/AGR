using RayTracer.World;

namespace RayTracer.Shading.Tracers
{
    public interface IRayTracer
    {
        Color3 Sample(Scene scene, Ray ray);
    }
}