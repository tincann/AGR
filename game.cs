#define PARALLEL

using System;
using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        private OpenTKApp _app;
        
        public void Init(OpenTKApp app, CancellationToken exitToken)
        {
            _exitToken = exitToken;
            _app = app;
            _tasks = new Task[parallelBundles];
            _r = RNG.CreateMultipleRNGs(parallelBundles);
            
            Screen.Clear(0x2222ff);
            _acc = new Accumulator(Screen);

            //var tracer = new WhittedStyleTracer();
            var tracer = new PathTracer();
            _scene = new Scene(tracer, constructBVH: true);
            _sceneManager = new SceneManager(_camera, _scene);

            _sceneManager.Add(SceneDefinitions.Default);
            _sceneManager.Add(SceneDefinitions.DarkRoom);
            _sceneManager.Add(SceneDefinitions.PathTracerBox);
            _sceneManager.Add(SceneDefinitions.BeerTest);
            _sceneManager.Add(SceneDefinitions.Teapot);
            
            _sceneManager.SetScene(2);

            Statistics.Enabled = false;
        }

        private float c = 0;
        private DateTime _startTime = DateTime.Now;
        public void Tick()
        {
            //Screen.Print($"d: {_camera.D}", 2, 2, 0xffffff);
            c += 0.001f;
            //_scene.PointLights.First().Position = new Vector3((float)Math.Sin(c) * 5, 1.5f, 0);
            
            if (Statistics.Enabled)
            {
                Screen.Print($"Triangle tests {Statistics.Get("Triangle test")}", 2, 42, 0x00ff00);
            }

            Screen.Print($"total: {(DateTime.Now - _startTime).TotalSeconds} sec", 2, 2, 0x00ff00);
            Screen.Print($"samples: {_acc.NumSamples}", 2, 42, 0x00ff00);
            Screen.Print($"spp: {SampleSize}", 410, 2, 0x00ff00);

            //Screen.Print($"gamma (kp_7, kp_8): {_gammaCorrection}", 2, 82, 0xffffff);
            
            Statistics.Reset();
        }

        readonly Stopwatch _sw = new Stopwatch();
        private Task[] _tasks;

        private int parallelBundles = 8;
        private int _sampleSizeX = 1;
        private int _sampleSizeY = 1;
        private int SampleSize => _sampleSizeX*_sampleSizeY;
        private bool _gammaCorrection = true;
        private RNG[] _r;
        private CancellationToken _exitToken;
        private SceneManager _sceneManager;

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
                            var color = TraceRay(x, y, _r[currentBundle]);
                            _acc.Plot(x, y, color, _gammaCorrection);
                        }
                    }
                });
            }

            while (!_exitToken.IsCancellationRequested && _tasks.Any(x => !x.IsCompleted))
            {
                _app.ProcessEvents();
                Task.WaitAny(_tasks, _exitToken);
            }

#else
            for (int y = 0; y < Screen.Height; y++)
            {
                //Console.WriteLine($"y: {y}");
                for (int x = 0; x < Screen.Width; x++)
                {
                    var color = TraceRay(x, y, _r[0]);
                    _acc.Plot(x, y, color, _gammaCorrection);
                }
            }
#endif
            _sw.Stop();
            _acc.EndFrame();
            Screen.Print($"time: {_sw.ElapsedMilliseconds}", 2, 22, 0x00ff00);
        }

        public Color3 TraceRay(int x, int y, RNG rng)
        {
            float v = (float)y / Screen.Height;
            float u = (float)x / Screen.Width;

            Color3 color = new Color3(0,0,0);
           
            var strat = new Stratifier(rng, _sampleSizeX, _sampleSizeY);
            for (int i = 0; i < SampleSize; i++)
            {
                var pos = strat.GetRandomPointInStratum(i);
                var dx = pos.X / Screen.Width;
                var dy = pos.Y / Screen.Height;
                var r = _camera.CreatePrimaryRay(u + dx, v + dy);
                color += _scene.Sample(r, rng, false);
            }

            color /= SampleSize;
            return color;
        }

        public void MoveCamera(Vector3 moveVector)
        {
            _camera.Move(moveVector);
            RestartSample();
        }

        public void RotateCamera(Vector2 rotVector)
        {
            _camera.Rotate(rotVector);
            RestartSample();
        }

        public void Antialiasing(int d)
        {
            _sampleSizeX = MathHelper.Clamp(_sampleSizeX + d, 1, 16);
            _sampleSizeY = _sampleSizeX; //always square samples
            Console.WriteLine($"Sample per pixels: {_sampleSizeX * _sampleSizeY}");
        }

        public void GammaCorrection(bool on)
        {
            _gammaCorrection = on;
            Console.WriteLine($"Gamme correction: {_gammaCorrection}");
        }

        public void PreviousScene()
        {
            _sceneManager.Previous();
            RestartSample();
            Console.WriteLine($"Scene {_sceneManager.CurrentScene}");
        }

        public void NextScene()
        {
            _sceneManager.Next();
            RestartSample();
            Console.WriteLine($"Scene {_sceneManager.CurrentScene}");
        }

        public void RestartSample()
        {
            Console.WriteLine("Resetting sampling");
            _startTime = DateTime.Now;
            _acc.Reset();
        }
    }
}
