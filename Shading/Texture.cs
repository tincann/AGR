using OpenTK;

namespace RayTracer.Shading
{
    public interface Texture
    {
        Color3 GetColor(Vector3 point);
    }
}
