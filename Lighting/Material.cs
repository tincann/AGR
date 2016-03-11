using RayTracer.World;

namespace RayTracer.Lighting
{
    public class Material
    {
        public Material(MaterialType materialType, Color3 color)
        {
            MaterialType = materialType;
            Color = color;
        }

        public MaterialType MaterialType { get; }
        public Color3 Color { get; }

        public float Specularity { get; set; } = 1;
        public float RefractiveIndex { get; set; } = 1;
    }
}
