using System;
using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.Lighting.Textures
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
            var hSize = _squareSize/2.0f;
            if ((Math.Abs(point.X) % _squareSize < hSize) == (Math.Abs(point.Z) % _squareSize < hSize))
            {
                return _c1;
            }

            return _c2;

        }
    }
}
