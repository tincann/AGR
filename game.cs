using OpenTK;
using RayTracer.World;

namespace RayTracer
{
    internal class Game
    {
        public Surface Screen;
        private Camera _camera = new Camera(Vector3.Zero, Vector3.UnitZ, 75);
        private Scene _scene = new Scene();

        public void Init()
        {
            Screen.Clear(0x2222ff);
            _scene.LightSource = new LightSource(new Vector3(10, 10, 10));
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