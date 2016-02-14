using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Template
{
    public class Sprite
    {
        public static Surface Target;
        private readonly Surface _bitmap;
        private readonly int _textureId;

        public Sprite(string fileName)
        {
            _bitmap = new Surface(fileName);
            _textureId = _bitmap.GenTexture();
        }

        public void Draw(float x, float y, float scale = 1.0f)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(PrimitiveType.Quads);
            var u1 = (x*2 - 0.5f*scale*_bitmap.Width)/Target.Width - 1;
            var v1 = 1 - (y*2 - 0.5f*scale*_bitmap.Height)/Target.Height;
            var u2 = ((x + 0.5f*scale*_bitmap.Width)*2)/Target.Width - 1;
            var v2 = 1 - ((y + 0.5f*scale*_bitmap.Height)*2)/Target.Height;
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(u1, v2);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(u2, v2);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(u2, v1);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(u1, v1);
            GL.End();
            GL.Disable(EnableCap.Blend);
        }
    }

    public class Surface
    {
        private static bool _fontReady;
        private static Surface _font;
        private static int[] _fontRedir;
        public int[] Pixels;
        public int Width, Height;

        public Surface(int w, int h)
        {
            Width = w;
            Height = h;
            Pixels = new int[w*h];
        }

        public Surface(string fileName)
        {
            var bmp = new Bitmap(fileName);
            Width = bmp.Width;
            Height = bmp.Height;
            Pixels = new int[Width*Height];
            var data = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            var ptr = data.Scan0;
            Marshal.Copy(data.Scan0, Pixels, 0, Width*Height);
            bmp.UnlockBits(data);
        }

        public int GenTexture()
        {
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, Pixels);
            return id;
        }

        public void Clear(int c)
        {
            for (int s = Width*Height, p = 0; p < s; p++) Pixels[p] = c;
        }

        public void Box(int x1, int y1, int x2, int y2, int c)
        {
            var dest = y1*Width;
            for (var y = y1; y <= y2; y++, dest += Width)
            {
                Pixels[dest + x1] = c;
                Pixels[dest + x2] = c;
            }
            var dest1 = y1*Width;
            var dest2 = y2*Width;
            for (var x = x1; x <= x2; x++)
            {
                Pixels[dest1 + x] = c;
                Pixels[dest2 + x] = c;
            }
        }

        public void Bar(int x1, int y1, int x2, int y2, int c)
        {
            var dest = y1*Width;
            for (var y = y1; y <= y2; y++, dest += Width)
                for (var x = x1; x <= x2; x++)
                {
                    Pixels[dest + x] = c;
                }
        }

        public void Line(int x1, int y1, int x2, int y2, int c)
        {
            if ((x1 < 0) || (y1 < 0) || (x2 < 0) || (y2 < 0) ||
                (x1 >= Width) || (x2 >= Width) || (y1 >= Height) || (y2 >= Height)) return;
            if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
            {
                if (x2 < x1)
                {
                    var h = x1;
                    x1 = x2;
                    x2 = h;
                    h = y2;
                    y2 = y1;
                    y1 = h;
                }
                var l = x2 - x1;
                var dy = ((y2 - y1)*8192)/l;
                y1 *= 8192;
                for (var i = 0; i < l; i++)
                {
                    Pixels[x1++ + (y1/8192)*Width] = c;
                    y1 += dy;
                }
            }
            else
            {
                if (y2 < y1)
                {
                    var h = x1;
                    x1 = x2;
                    x2 = h;
                    h = y2;
                    y2 = y1;
                    y1 = h;
                }
                var l = y2 - y1;
                var dx = ((x2 - x1)*8192)/l;
                x1 *= 8192;
                for (var i = 0; i < l; i++)
                {
                    Pixels[x1/8192 + y1++*Width] = c;
                    x1 += dx;
                }
            }
        }

        public void Plot(int x, int y, int c)
        {
            if ((x >= 0) && (y >= 0) && (x < Width) && (y < Height))
            {
                Pixels[x + y*Width] = c;
            }
        }

        public void Print(string t, int x, int y, int c)
        {
            if (!_fontReady)
            {
                _font = new Surface("../../assets/font.png");
                var ch = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+={}[];:<>,.?/\\ ";
                _fontRedir = new int[256];
                for (var i = 0; i < 256; i++) _fontRedir[i] = 0;
                for (var i = 0; i < ch.Length; i++)
                {
                    int l = ch[i];
                    _fontRedir[l & 255] = i;
                }
                _fontReady = true;
            }
            for (var i = 0; i < t.Length; i++)
            {
                var f = _fontRedir[t[i] & 255];
                var dest = x + i*12 + y*Width;
                var src = f*12;
                for (var v = 0; v < _font.Height; v++, src += _font.Width, dest += Width)
                    for (var u = 0; u < 12; u++)
                    {
                        if ((_font.Pixels[src + u] & 0xffffff) != 0) Pixels[dest + u] = c;
                    }
            }
        }
    }
}