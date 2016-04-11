using OpenTK;
using RayTracer.Shading;

namespace RayTracer.World.Ambiance
{
    public interface Skybox
    {
        Color3 Intersect(Vector3 direction);
    }
}
