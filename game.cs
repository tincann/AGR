#define PARALLEL

using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization.Configuration;
using RayTracer.Helpers;
using RayTracer.Shading;

namespace RayTracer
{
    internal class Game
    {
        private readonly Camera _camera = new Camera(new Vector3(3, 5, 0), Vector3.UnitZ);
        private readonly Scene _scene = new Scene();
        public Surface Screen;

        public void Init()
        {
            tasks = new Task[parallelBundles];
            Screen.Clear(0x2222ff);
            
            var sceneDef = new SceneDefinition(_camera, _scene);

            //sceneDef.Default();
            //sceneDef.Teapot();
            sceneDef.BeerTest();

            _scene.Construct();

            Statistics.Enabled = false;
        }
        
        public void Tick()
        {
            Screen.Print($"d: {_camera.D}", 2, 2, 0xffffff);
            
            if (Statistics.Enabled)
            {
                Screen.Print($"Triangle tests {Statistics.Get("Triangle test")}", 2, 42, 0xffffff);
            }

            Screen.Print($"spp (kp_+, kp_-): {_sampleSize}", 2, 42, 0xffffff);
            Screen.Print($"gamma (kp_7, kp_8): {_gammaCorrection}", 2, 62, 0xffffff);

            Statistics.Reset();
        }

        readonly Stopwatch _sw = new Stopwatch();
        private Task[] tasks;

        private int parallelBundles = 32;
        private int _sampleSize = 1;
        private bool _gammaCorrection = true;

        public void Render()
        {
            _sw.Restart();
            // render stuff over the backbuffer (OpenGL, sprites)

#if PARALLEL
            var bundleSize = Screen.Height/parallelBundles;
            for (int b = 0; b < parallelBundles; b++)
            {
                //Console.WriteLine($"y: {y}");
                int currentBundle = b;
                tasks[b] = Task.Factory.StartNew(() =>
                {
                    var beginLine = currentBundle*bundleSize;
                    for (int y = beginLine; y < beginLine + bundleSize; y++)
                    {
                        for (int x = 0; x < Screen.Width; x++)
                        {
                            TraceRay(x, y);
                        }
                    }
                });
            }
            Task.WaitAll(tasks);
            
#else
            for (int y = 0; y < Screen.Height; y++)
            {
                //Console.WriteLine($"y: {y}");
                for (int x = 0; x < Screen.Width; x++)
                {
                    TraceRay(x, y);
                }
            }
#endif
            _sw.Stop();
            Screen.Print($"time: {_sw.ElapsedMilliseconds}", 2, 22, 0xffffff);
        }

        public void TraceRay(int x, int y)
        {
            float v = (float)y / Screen.Height;
            float u = (float)x / Screen.Width;

            var xSize = 1.0f/Screen.Width;
            var ySize = 1.0f/Screen.Height;

            Color3 color = new Color3(0,0,0);
            for (int i = 0; i < _sampleSize; i++)
            {
                var dx = xSize*i/_sampleSize;
                var dy = ySize*i/_sampleSize;
                var ray = _camera.CreatePrimaryRay(u + dx, v + dy);
                color += _scene.Intersect(ray);
            }

            color /= _sampleSize;
            
            Screen.Plot(x, y, color.ToArgb(_gammaCorrection));
        }

        public void MoveCamera(Vector3 moveVector)
        {
            _camera.Move(moveVector);
        }

        public void RotateCamera(Vector2 rotVector)
        {
            _camera.Rotate(rotVector);
        }


        public void Antialiasing(int d)
        {
            _sampleSize = MathHelper.Clamp(_sampleSize + d, 1, 16);
        }

        public void GammaCorrection(bool on)
        {
            _gammaCorrection = on;
        }
    }
}
