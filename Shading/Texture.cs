using OpenTK;

namespace RayTracer.Shading
{
    public interface Texture
    {
        Color3 GetColor(Vector3 point); //todo - procedural texture generation doesn't belong in Texture interface
        Color3 GetColor(Vector2 uv);
    }
}
