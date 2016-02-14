using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
    internal class Game
    {
        public Surface Screen;

        public void Init()
        {
            Screen.Clear(0x2222ff);
        }

        public void Tick()
        {
            Screen.Print("hello world!", 2, 2, 0xffffff);
        }

        public void Render()
        {
            // render stuff over the backbuffer (OpenGL, sprites)
        }
    }
} // namespace Template