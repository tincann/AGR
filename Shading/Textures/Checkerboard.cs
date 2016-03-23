using System;
using OpenTK;

namespace RayTracer.Shading.Textures
{
    public class Checkerboard : Texture
    {
        private readonly int _squareSize;
        private readonly Color3 _c1;
        private readonly Color3 _c2;

        public Checkerboard(int squareSize, Color3 c1, Color3 c2)
        {
            _squareSize = squareSize;
            _c1 = c1;
            _c2 = c2;
        }

        public Color3 GetColor(Vector3 point)
        {
            return GetColor(point.Xz);
        }

        public Color3 GetColor(Vector2 uv)
        {
            if (((int)Math.Floor(uv.X / _squareSize) % 2 == 0) == ((int)Math.Floor(uv.Y /_squareSize) % 2 == 0))
            {
                return _c1;
            }

            return _c2;
        }
    }
}
