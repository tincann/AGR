﻿using System;
using OpenTK;
using RayTracer.World;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenTK.Graphics;
using RayTracer.Lighting;
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
            Screen.Clear(0x2222ff);
            _scene.LightSources.Add(new LightSource(new Vector3(0, 6, 3), Color4.White));
            _scene.Objects.Add(new Triangle(
                new Vector3(-0.5f, 0, 1),
                new Vector3(0, 1, 1),
                new Vector3(0.5f, 0, 1),
                MaterialType.Diffuse,
                new Color3(Color4.Aqua)
                ));

            _scene.Objects.Add(new Triangle(
                new Vector3(5, 0, 5),
                new Vector3(5, 0, -5),
                new Vector3(-5, 0, -5),
                MaterialType.Diffuse,
                new Color3(Color4.Bisque)
            ));
            _scene.Objects.Add(new Triangle(
                new Vector3(5, 0, 5),
                new Vector3(-5, 0, -5),
                new Vector3(-5, 0, 5),
                MaterialType.Diffuse,
                new Color3(Color4.Bisque)
            ));

            //var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj");
            //_scene.Objects.Add(teapot);

            var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj", MaterialType.Specular, new Color3(Color4.Green));
            _scene.Objects.Add(cube);
        }

        private static float i = 0;
        public void Tick()
        {
            
            var radius = 2;
            _camera.Update(new Vector3((float)Math.Sin(i) * radius, (float)Math.Sin(i) * radius + 0.5f, (float)Math.Cos(i) * radius));
            //_scene.LightSources[0] = new LightSource(new Vector3(0, (float)Math.Sin(i) * radius + 2, 2), Color4.White);
            //_camera.d = (float)(Math.Sin(i) * 0.5 + 1);
            //_camera.Update();
            Screen.Print($"d: {_camera.d}", 2, 2, 0xffffff);

            i += 0.01f;
        }

        readonly Stopwatch _sw = new Stopwatch();
        public void Render()
        {
            _sw.Restart();
            Task[] tasks = new Task[Screen.Height];
            // render stuff over the backbuffer (OpenGL, sprites)
            for (int y = 0; y < Screen.Height; y++)
            {
                //Console.WriteLine($"y: {y}");
                int copyY = y;
                tasks[y] = Task.Factory.StartNew(() =>
                {
                    float v = (float)copyY / Screen.Height;
                    for (int x = 0; x < Screen.Width; x++)
                    {
                        float u = (float)x / Screen.Width;
                        var ray = _camera.CreatePrimaryRay(u, v);

                        var color = _scene.Intersect(ray);
                        Screen.Plot(x, copyY, color.ToArgb());
                    }
                });
            }
            while (true)
            {
                var count = tasks.Count(x => !x.IsCompleted);
                Console.WriteLine($"Rays remaining: {count}");
                if (count < 100)
                {
                    break;
                }

                //Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            Task.WaitAll(tasks);
            _sw.Stop();
            Screen.Print($"time: {_sw.ElapsedMilliseconds}", 2, 22, 0xffffff);
        }
    }
}
