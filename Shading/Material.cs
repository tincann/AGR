using System;
using OpenTK.Graphics;

namespace RayTracer.Shading
{
    public class Material
    {
        public Material(MaterialType materialType)
        {
            MaterialType = materialType;
        }

        public Material WithColor(Color3 color)
        {
            var copy = MemberwiseClone() as Material;
            if(copy == null) throw new Exception("This shouldn't happen");
            copy.Color = color;
            return copy;
        }

        public MaterialType MaterialType { get; }
        public Color3 Color { get; set; } = Color4.White;

        public Texture Texture { get; set; }
        public float Specularity { get; set; } = 1;
        public float Diffusivity => 1 - Specularity;
        public float RefractiveIndex { get; set; } = 1;
        public float Absorbance { get; set; } = 1;

        public static readonly Material Air = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.0f, Absorbance = 0};
        public static readonly Material Glass = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.4f, Absorbance = 0.2f };
        public static readonly Material Water = new Material(MaterialType.Dielectric) { RefractiveIndex = 1.35f, Absorbance = 0.2f };
        public static readonly Material Metal = new Material(MaterialType.Specular) { Color = Color4.Gray,Specularity = 0.8f };
        public static readonly Material Light = new Material(MaterialType.Light) { Color = Color4.White };
    }
}
