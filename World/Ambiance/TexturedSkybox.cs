using System;
using OpenTK;
using RayTracer.Shading;
using RayTracer.Shading.Textures;

namespace RayTracer.World.Ambiance
{
    public class TexturedSkybox : Skybox
    {
        private readonly ImageTexture _texture;
        public TexturedSkybox(string path)
        {
            _texture = new ImageTexture(path);
        }

        public Color3 Intersect(Vector3 direction)
        {
            var u = (1 + Math.Atan2(direction.X, -direction.Z)/MathHelper.Pi)/ 2;
            var v = Math.Acos(direction.Y)/MathHelper.Pi;
            var color = _texture.GetColor((float)u, (float)v);
            return color;
        }
    }
}