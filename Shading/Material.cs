using OpenTK.Graphics;

namespace RayTracer.Shading
{
    public class Material
    {
        public Material(MaterialType materialType)
        {
            MaterialType = materialType;
        }

        public MaterialType MaterialType { get; }
        public Color3 Color { get; set; } = new Color3(Color4.White);

        public Texture Texture { get; set; }
        public float Specularity { get; set; } = 1;
        public float RefractiveIndex { get; set; } = 1;
        public float Absorbance { get; set; } = 1;

        public static readonly Material Air = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.0f, Absorbance = 0};
        public static readonly Material Glass = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.4f, Absorbance = 0.2f };
        public static readonly Material Water = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.35f, Absorbance = 0.2f };
        public static readonly Material Metal = new Material(MaterialType.Specular) { Color = new Color3(Color4.Gray),Specularity = 0.8f };
    }
}
