using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace RayTracer.World
{
    public struct Material
    {
        public MaterialType Type { get; set; }
        public Color4 Color { get; set; }
    }

    public enum MaterialType
    {
        Diffuse,
        Specular
    }
}
