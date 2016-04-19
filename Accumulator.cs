using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Shading;

namespace RayTracer
{
    public class Accumulator
    {
        private readonly Surface _screen;
        private Color3[,] _acc;
        public int NumSamples { get; private set; }

        public Accumulator(Surface screen)
        {
            _screen = screen;
            _acc = new Color3[screen.Width, screen.Height];
            for(int y = 0; y < screen.Height; y++)
                for (int x = 0; x < screen.Width; x++)
                {
                    _acc[x,y] = new Color3(0,0,0);
                }
        }

        public void EndFrame()
        {
            NumSamples++;
        }

        public void Reset()
        {
            NumSamples = 0;
        }

        public void Plot(int x, int y, Color3 color, bool gammaCorrection)
        {
            if (float.IsNaN(color.R)) return;
            
            var oldColor = _acc[x, y];
            var newColor = (oldColor*NumSamples + color)/(NumSamples + 1);
            _acc[x, y] = newColor;
            _screen.Plot(x, y, newColor.ToArgb(gammaCorrection));
        }
    }
}
