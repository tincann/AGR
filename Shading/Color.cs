using System;
using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.Shading
{
    public class Color3
    {
        public float B;
        public float G;
        public float R;

        public Color3(byte r, byte g, byte b)
        {
            R = (float)r/255;
            G = (float)g/255;
            B = (float)b/255;
        }

        public Color3(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
        public Color3(Color4 color)
        {
            B = color.B;
            G = color.G;
            R = color.R;
        }

        public static implicit operator Color3(Color4 color)
        {
            return new Color3(color.R, color.G, color.B);
        }

        public static Color3 operator +(Color3 c1, Color3 c2)
        {
            return new Color3(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        }

        public static Color3 operator *(Color3 color, float s)
        {
            return new Color3(color.R * s, color.G * s, color.B * s);
        }

        public static Color3 operator *(Vector3 vector, Color3 color)
        {
            return new Color3(color.R * vector.X, color.G * vector.Y, color.B * vector.Z);
        }

        public static Color3 operator *(Color3 c1, Color3 c2)
        {
            return new Color3(c1.R * c2.R, c1.G * c2.G, c1.B * c2.B);
        }

        public static Color3 operator -(Color3 c1, Color3 c2)
        {
            return new Color3(c1.R - c2.R, c1.G - c2.G, c1.B - c2.B);
        }

        public static Color3 operator /(Color3 color, float denominator)
        {
            return new Color3(color.R / denominator, color.G / denominator, color.B / denominator);
        }

        public static Color3 operator *(float s, Color3 color)
        {
            return color*s;
        }

        public int ToArgb(bool gammaCorrect)
        {
            float r = R, g = G, b = B;
            if (gammaCorrect)
            {
                r = (float)Math.Sqrt(R);
                g = (float)Math.Sqrt(G);
                b = (float)Math.Sqrt(B);
            }
            return new Color4(Math.Min(r, 1), Math.Min(g, 1), Math.Min(b, 1), 1).ToArgb();
        }
    }
}
