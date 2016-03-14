﻿using System;
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
            var hSize = _squareSize / 2.0f;
            if ((Math.Abs(uv.X) % _squareSize < hSize) == (Math.Abs(uv.Y) % _squareSize < hSize))
            {
                return _c1;
            }

            return _c2;
        }
    }
}
