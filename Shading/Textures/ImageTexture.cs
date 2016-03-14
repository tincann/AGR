using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK;

namespace RayTracer.Shading.Textures
{
    public class ImageTexture : Texture
    {
        private readonly byte[] _imageBuffer;
        private readonly int _width;
        private readonly int _height;
        private readonly int _depth;

        public ImageTexture(string path)
        {
            //load image from file
            var image = Bitmap.FromFile(path) as Bitmap;
            _width = image.Width;
            _height = image.Height;

            //lock file in memory
            var bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                image.PixelFormat);
            _depth = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;

            //copy to in-memory buffer
            _imageBuffer = new byte[bitmapData.Width * bitmapData.Height * _depth];
            Marshal.Copy(bitmapData.Scan0, _imageBuffer, 0, _imageBuffer.Length);

            //release lock and dispose
            image.UnlockBits(bitmapData);
            image.Dispose();
        }

        public Color3 GetColor(Vector3 point)
        {
            throw new NotImplementedException();
        }

        public Color3 GetColor(Vector2 uv)
        {
            return GetColor(uv.X, uv.Y);
        }

        public Color3 GetColor(float u, float v)
        {
            Debug.Assert(u < _width && v < _height);
            Debug.Assert(u >= 0 && u <= 1 && v >= 0 && v <= 1);

            int x = (int) (u*_width);
            int y = (int) (v*_height);

            var offset = (y*_width + x)*_depth;
            return new Color3(_imageBuffer[offset + 2], _imageBuffer[offset + 1], _imageBuffer[offset]);
        }
    }
}
