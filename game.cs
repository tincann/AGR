using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template {

class Game
{
	public Surface screen;
	public void Init()
	{
		screen.Clear( 0x2222ff );
	}
	public void Tick()
	{
		screen.Print( "hello world!", 2, 2, 0xffffff );
	}
	public void Render()
	{
		// render stuff over the backbuffer (OpenGL, sprites)
        
	}
}

} // namespace Template
