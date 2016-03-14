#define PARALLEL

using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Textures;
using RayTracer.World.Objects;

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
            _scene.Add(new LightSource(new Vector3(5, 6, 3), new Color3(Color4.White), 20));
            _scene.Add(new LightSource(new Vector3(-2000, 2000, 2000), new Color3(Color4.White), 7000000));

            _scene.Add(new Plane(
                Vector3.UnitY, 
                0,
                new Material(MaterialType.Diffuse) { Texture = new Checkerboard(5, new Color3(Color4.Bisque), new Color3(Color4.White))}
            ));

            //var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj", mat);
            //_scene.Add(new BoundingVolumeHierarchy(teapot.Triangles).Root);

            //var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj",
            //    new Vector3(3, 0.5f, 1),
            //    Material.Glass);
            //_scene.Add(cube);

            _scene.Add(new Sphere(new Vector3(1, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Red), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(2, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Green), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 0.5f, -2f), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Blue), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 1.25f, -1.5f), 0.5f,
                new Material(MaterialType.Diffuse) { Specularity = 0.95f }));
            
            _scene.Add(new Sphere(new Vector3(0, 1, 2), 1, Material.Metal));
            
            _scene.Construct();

            Statistics.Enabled = false;
        }

        private static float i = 0;
        public void Tick()
        {
            var radius = 6;
            //_camera.Update(new Vector3((float)Math.Sin(i) * radius, (float)Math.Sin(i) * radius / 2 + 1.6f, (float)Math.Cos(i) * radius));
            //_scene.LightSources[0] = new LightSource(new Vector3((float)Math.Sin(i * 10) * radius + 2, 5, (float)Math.Sin(i * 10) * radius + 2), Color4.White);
            //_camera.d = (float)(Math.Sin(i) * 0.5 + 1);
            //_camera.Update();
            Screen.Print($"d: {_camera.D}", 2, 2, 0xffffff);

            if (Statistics.Enabled)
            {
                Screen.Print($"Triangle tests {Statistics.Get("Triangle test")}", 2, 42, 0xffffff);
            }

            Statistics.Reset();
            i += 0.03f;
        }

        readonly Stopwatch _sw = new Stopwatch();
        private Task[] tasks;

        private int parallelBundles = 32;
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
            var ray = _camera.CreatePrimaryRay(u, v);

            var color = _scene.Intersect(ray);
            
            Screen.Plot(x, y, color.ToArgb(true));
        }

        public void MoveCamera(Vector3 moveVector)
        {
            _camera.Move(moveVector);
        }

        public void RotateCamera(Vector2 rotVector)
        {
            _camera.Rotate(rotVector);
        }
    }
}
