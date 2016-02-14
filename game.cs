using OpenTK;
using OpenTK.Graphics;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer
{
    internal class Game
    {
        private readonly Camera _camera = new Camera(Vector3.Zero, Vector3.UnitZ, 75);
        private readonly Scene _scene = new Scene();
        public Surface Screen;

        public void Init()
        {
            Screen.Clear(0x2222ff);
            _scene.LightSource = new LightSource(new Vector3(10, 10, 10));
            _scene.Objects.Add(new Triangle(
                new Vector3(-0.5f, 0, 2),
                new Vector3(0, 1, 2),
                new Vector3(0.5f, 0, 2),
                MaterialType.Diffuse,
                Color4.Aqua
                ));
        }

        public void Tick()
        {
            Screen.Print("hello world!", 2, 2, 0xffffff);
        }

        public void Render()
        {
            // render stuff over the backbuffer (OpenGL, sprites)
            for (int y = 0; y < Screen.Height; y++)
            {
                float v = (float) y/Screen.Height;
                for (int x = 0; x < Screen.Width; x++)
                {
                    float u = (float) x/Screen.Width;
                    var ray = _camera.CreatePrimaryRay(u, v);

                    var color = _scene.Intersect(ray);
                    Screen.Plot(x, y, color.ToArgb());
                }
            }
            
        }
    }
}
