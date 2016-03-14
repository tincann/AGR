using System;
using OpenTK;
using RayTracer.Shading;
using RayTracer.Shading.Textures;

namespace RayTracer.World
{
    public class Skybox
    {
        private readonly ImageTexture _texture;
        public Skybox(string path)
        {
            _texture = new ImageTexture(path);
        }

        public Color3 Intersect(Vector3 direction)
        {
            var u = (1 + Math.Atan2(direction.X, -direction.Z)/Math.PI)/ 2;
            var v = Math.Acos(direction.Y)/Math.PI;
            var color = _texture.GetColor((float)u, (float)v);
            return color;
        }
    }
}
