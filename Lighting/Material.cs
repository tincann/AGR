using OpenTK.Graphics;
using RayTracer.World;

namespace RayTracer.Lighting
{
    public class Material
    {
        public Material(MaterialType materialType)
        {
            MaterialType = materialType;
        }

        public MaterialType MaterialType { get; }
        public Color3 Color { get; set; } = new Color3(Color4.White);

        public float Specularity { get; set; } = 1;
        public float RefractiveIndex { get; set; } = 1;
        
        public static readonly Material Air = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.0f };
        public static readonly Material Glass = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.5f };
        public static readonly Material Water = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.35f };
        public static readonly Material Metal = new Material(MaterialType.Specular) { Color = new Color3(Color4.Gray),Specularity = 0.8f };
    }
}
