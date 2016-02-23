using RayTracer.World;

namespace RayTracer.Lighting
{
    public struct Material
    {

        public Material(MaterialType materialType, Color3 color) : this(materialType, color, 1)
        {
        }

        public Material(MaterialType materialType, Color3 color, float reflectionPercentage)
        {
            MaterialType = materialType;
            Color = color;
            ReflectionPercentage = reflectionPercentage;
        }

        public MaterialType MaterialType { get; }
        public Color3 Color { get; }
        public float ReflectionPercentage { get; }
    }
}
