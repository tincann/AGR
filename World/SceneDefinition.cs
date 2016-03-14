﻿using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Textures;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class SceneDefinition
    {
        private readonly Camera _camera;
        private readonly Scene _scene;

        public SceneDefinition(Camera camera, Scene scene)
        {
            _camera = camera;
            _scene = scene;
        }

        private void AddFloor()
        {
            _scene.Add(new Plane(
                Vector3.UnitY,
                0,
                new Material(MaterialType.Diffuse) { Texture = new Checkerboard(5, new Color3(Color4.Bisque), new Color3(Color4.White)) }
            ));
        }

        public void AddLight()
        {
            _scene.Add(new LightSource(new Vector3(5, 6, 3), new Color3(Color4.White), 20));
            _scene.Add(new LightSource(new Vector3(-2000, 2000, 2000), new Color3(Color4.White), 7000000));
        }

        public void Teapot()
        {
            AddFloor();
            AddLight();
            var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj", Vector3.Zero, Material.Metal);
            _scene.Add(teapot);
        }

        public void Default()
        {
            AddFloor();
            AddLight();
            
            var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj",
                new Vector3(3, 0.51f, 1),
                Material.Glass);
            _scene.Add(cube);

            _scene.Add(new Sphere(new Vector3(1, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Red), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(2, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Green), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 0.5f, -1.9f), 0.5f,
                new Material(MaterialType.Diffuse) { Color = new Color3(Color4.Blue), Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 1.25f, -1.4f), 0.5f,
                new Material(MaterialType.Diffuse) { Specularity = 0.95f }));

            var mat = Material.Glass;
            mat.Color = new Color3(Color4.Green);
            _scene.Add(new Sphere(new Vector3(0, 1, 2), 1, mat));
            _scene.Add(new Sphere(new Vector3(0, 3, 2), 1, Material.Metal));
        }

        public void BeerTest()
        {
            _camera.Update(new Vector3(3.61508f, 2.465492f, 8.432084f), new Vector3(3.699683f, 2.259647f, 7.457158f));
            AddFloor();
            AddLight();

            var mat = Material.Glass;
            mat.Color = new Color3(Color4.Red);

            _scene.Add(new Sphere(new Vector3(0, 0.2f, 0), 0.2f, mat));
            _scene.Add(new Sphere(new Vector3(1, 0.4f, 0), 0.4f, mat));
            _scene.Add(new Sphere(new Vector3(2.5f, 0.8f, 0), 0.8f, mat));
            _scene.Add(new Sphere(new Vector3(5, 1.6f, 0), 1.6f, mat));
        }
    }
}
