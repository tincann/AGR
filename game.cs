using RayTracer.World;

namespace RayTracer
{
    internal class Game
    {
        public Surface Screen;
        public Camera Camera;
        public Scene Scene;

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