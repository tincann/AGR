using System;
using OpenTK.Graphics;

namespace RayTracer.World.Objects
{
    public class Background : Primitive
    {
        public Background(MaterialType materialType, Color4 color) : base(materialType, color)
        {
        }

        public override bool Intersect(Ray ray, out float t)
        {
            throw new NotImplementedException();
        }
    }
}
