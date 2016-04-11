//#define PARALLEL

using System;
using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Tracers;

namespace RayTracer
{
    internal class Game
    {
        private readonly Camera _camera = new Camera(new Vector3(3, 5, 0), Vector3.UnitZ);
        private Scene _scene;
        public Surface Screen;
        private Accumulator _acc;
        
        public void Init()
        {
            _tasks = new Task[parallelBundles];
            Screen.Clear(0x2222ff);
            _acc = new Accumulator(Screen);

            //var tracer = new WhittedStyleTracer();
            var tracer = new PathTracer();
            _scene = new Scene(tracer, true);
            var sceneDef = new SceneDefinition(_camera, _scene);

            //sceneDef.Default();
            //sceneDef.Teapot();
            //sceneDef.BeerTest();
            //sceneDef.PathTracerTest();
            sceneDef.PathTracerBox();

            _scene.Construct();

            Statistics.Enabled = false;
        }

        private float c = 0;
        public void Tick()
        {
            Screen.Print($"d: {_camera.D}", 2, 2, 0xffffff);
            c += 0.001f;
            //_scene.PointLights.First().Position = new Vector3((float)Math.Sin(c) * 5, 1.5f, 0);
            
            if (Statistics.Enabled)
            {
                Screen.Print($"Triangle tests {Statistics.Get("Triangle test")}", 2, 42, 0xffffff);
            }

            Screen.Print($"spp (kp_+, kp_-): {_sampleSize}", 2, 42, 0xffffff);
            Screen.Print($"gamma (kp_7, kp_8): {_gammaCorrection}", 2, 62, 0xffffff);

            Statistics.Reset();
        }

        readonly Stopwatch _sw = new Stopwatch();
        private Task[] _tasks;

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
                _tasks[b] = Task.Factory.StartNew(() =>
                {
                    var beginLine = currentBundle*bundleSize;
                    for (int y = beginLine; y < beginLine + bundleSize; y++)
                    {
                        for (int x = 0; x < Screen.Width; x++)
                        {
                            var color = TraceRay(x, y);
                            _acc.Plot(x, y, color, _gammaCorrection);
                            //Screen.Plot(x, y, color.ToArgb(_gammaCorrection));
                        }
                    }
                });
            }
            
            Task.WaitAll(_tasks);
            
#else
            for (int y = 0; y < Screen.Height; y++)
            {
                //Console.WriteLine($"y: {y}");
                for (int x = 0; x < Screen.Width; x++)
                {
                    var color = TraceRay(x, y);
                    _acc.Plot(x, y, color, _gammaCorrection);
                }
            }
#endif
            _sw.Stop();
            _acc.EndFrame();
            Screen.Print($"time: {_sw.ElapsedMilliseconds}", 2, 22, 0xffffff);
        }

        public Color3 TraceRay(int x, int y)
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
                color += _scene.Sample(ray);
            }

            color /= _sampleSize;
            return color;
        }

        public void MoveCamera(Vector3 moveVector)
        {
            _camera.Move(moveVector);
            _acc.Reset();
        }

        public void RotateCamera(Vector2 rotVector)
        {
            _camera.Rotate(rotVector);
            _acc.Reset();
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
