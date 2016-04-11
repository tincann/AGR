using OpenTK;
using RayTracer.Shading;

namespace RayTracer.World.Ambiance
{
    public class SingleColorSkybox : Skybox
    {
        private readonly Color3 _color;

        public SingleColorSkybox(Color3 color)
        {
            _color = color;
        }

        public Color3 Intersect(Vector3 direction)
        {
            return _color;
        }
    }
}