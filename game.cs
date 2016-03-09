#define PARALLEL

using System;
using System.Collections.Generic;
using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Lighting;
using RayTracer.Structures;
using RayTracer.World.Objects;

namespace RayTracer
{
    internal class Game
    {
        private readonly Camera _camera = new Camera(new Vector3(3, 5, 0), new Vector3(0, 0, 0), 75);
        private readonly Scene _scene = new Scene();
        public Surface Screen;

        public void Init()
        {
            tasks = new Task[Screen.Height];
            Screen.Clear(0x2222ff);
            _scene.LightSources.Add(new LightSource(new Vector3(0, 6, 3), Color4.White, 30));
            _scene.LightSources.Add(new LightSource(new Vector3(3, 3, 5), Color4.White, 30));

            _scene.Objects.Add(new Triangle(
                new Vector3(5, 0, 5),
                new Vector3(5, 0, -5),
                new Vector3(-5, 0, -5),
                new Material(
                    MaterialType.Diffuse,
                    new Color3(Color4.Bisque))
            ));
            _scene.Objects.Add(new Triangle(
                new Vector3(5, 0, 5),
                new Vector3(-5, 0, -5),
                new Vector3(-5, 0, 5),
                new Material(
                    MaterialType.Diffuse,
                    new Color3(Color4.Bisque))
            ));


            var mat = new Material(MaterialType.Mirror, new Color3(Color4.Green), 0.3f);
            //var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj", mat);
            //_scene.Objects.Add(teapot);

            var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj", mat);
            var bvh = new BoundingVolumeHierarchy(cube.Triangles);
            _scene.Objects.Add(bvh.Root);
            
            var balls = new List<Boundable>();
            balls.Add(new Sphere(new Vector3(1, 0.5f, -1), 0.5f,
                new Material(MaterialType.Mirror, new Color3(Color4.Red), 0.9f)));
            balls.Add(new Sphere(new Vector3(2, 0.5f, -1), 0.5f,
                new Material(MaterialType.Mirror, new Color3(Color4.Green), 0.9f)));
            balls.Add(new Sphere(new Vector3(1.5f, 0.5f, -2), 0.5f,
                new Material(MaterialType.Mirror, new Color3(Color4.Blue), 0.9f)));
            balls.Add(new Sphere(new Vector3(1.5f, 1.25f, -1.5f), 0.5f,
                new Material(MaterialType.Mirror, new Color3(Color4.White), 0.95f)));

            var bvh2 = new BoundingVolumeHierarchy(balls);
            _scene.Objects.Add(bvh2.Root);

            Statistics.Enabled = false;
        }

        private static float i = 0;
        public void Tick()
        {
            var radius = 3;
            _camera.Update(new Vector3((float)Math.Sin(i) * radius, (float)Math.Sin(i) * radius + 0.5f, (float)Math.Cos(i) * radius));
            //_scene.LightSources[0] = new LightSource(new Vector3((float)Math.Sin(i * 10) * radius + 2, 5, (float)Math.Sin(i * 10) * radius + 2), Color4.White);
            //_camera.d = (float)(Math.Sin(i) * 0.5 + 1);
            //_camera.Update();
            Screen.Print($"d: {_camera.d}", 2, 2, 0xffffff);

            if (Statistics.Enabled)
            {
                Screen.Print($"Triangle tests {Statistics.Get("Triangle test")}", 2, 42, 0xffffff);
            }

            Statistics.Reset();
            i += 0.03f;
        }

        readonly Stopwatch _sw = new Stopwatch();
        private Task[] tasks;

        public void Render()
        {
            _sw.Restart();
            // render stuff over the backbuffer (OpenGL, sprites)

#if PARALLEL
            for (int y = 0; y < Screen.Height; y++)
            {
                //Console.WriteLine($"y: {y}");
                int copyY = y;
                tasks[y] = Task.Factory.StartNew(() =>
                {     
                    for (int x = 0; x < Screen.Width; x++)
                    {
                        TraceRay(x, copyY);
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
            //while (true)
            //{
            //    var count = tasks.Count(x => !x.IsCompleted);
            //    Console.WriteLine($"Rays remaining: {count}");
            //    if (count < 100)
            //    {
            //        break;
            //    }

            //    //Thread.Sleep(TimeSpan.FromSeconds(5));
            //}
            _sw.Stop();
            Screen.Print($"time: {_sw.ElapsedMilliseconds}", 2, 22, 0xffffff);
        }

        public void TraceRay(int x, int y)
        {
            float v = (float)y / Screen.Height;
            float u = (float)x / Screen.Width;
            var ray = _camera.CreatePrimaryRay(u, v);

            var color = _scene.Intersect(ray);
            Screen.Plot(x, y, color.ToArgb());
        }
    }
}
