using System.IO;
using OpenTK;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Textures;
using RayTracer.World.Ambiance;
using RayTracer.World.Objects.Complex;
using RayTracer.World.Objects.Primitives;

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

        private void AddSkybox()
        {
            _scene.Skybox = new TexturedSkybox(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\assets\\skybox3.jpg"));
        }

        private void AddFloor()
        {
            _scene.Add(new Plane(
                Vector3.UnitY,
                0,
                new Material(MaterialType.Diffuse) { Texture = new Checkerboard(5, Color4.Bisque, Color4.White) }
            ));
        }

        public void AddLight()
        {
            _scene.Add(new PointLight(new Vector3(5, 6, 3), Color4.White, 20));
            _scene.Add(new PointLight(new Vector3(-2000, 2000, 2000), Color4.White, 7000000));
        }

        public void Teapot()
        {
            AddSkybox();
            AddFloor();
            AddLight();
            var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj", Vector3.Zero, Material.Metal);
            _scene.Add(teapot);
        }

        public void Default()
        {
            AddSkybox();
            AddFloor();
            AddLight();
            
            //var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj",
            //    new Vector3(3, 0.51f, 1),
            //    Material.Glass);
            //_scene.Add(cube);
            
            _scene.Add(new Sphere(new Vector3(1, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Red, Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(2, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Green, Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 0.5f, -1.9f), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Blue, Specularity = 0.9f }));
            _scene.Add(new Sphere(new Vector3(1.5f, 1.25f, -1.4f), 0.5f,
                new Material(MaterialType.Diffuse) { Specularity = 0.95f }));
            
            _scene.Add(new Sphere(new Vector3(0, 1, 2), 1, Material.Glass.WithColor(Color4.Red)));
            _scene.Add(new Sphere(new Vector3(0, 3, 2), 1, Material.Metal));
        }

        public void BeerTest()
        {
            AddSkybox();
            _camera.Update(new Vector3(3.61508f, 2.465492f, 8.432084f), new Vector3(3.699683f, 2.259647f, 7.457158f));
            AddFloor();
            AddLight();

            var mat = Material.Glass;
            mat.Color = Color4.Red;
            
            _scene.Add(new Sphere(new Vector3(0, 0.2f, 0), 0.2f, mat));
            _scene.Add(new Sphere(new Vector3(1, 0.4f, 0), 0.4f, mat));
            _scene.Add(new Sphere(new Vector3(2.5f, 0.8f, 0), 0.8f, mat));
            _scene.Add(new Sphere(new Vector3(5, 1.6f, 0), 1.6f, mat));
        }

        public void PathTracerBox()
        {
            //AddSkybox();
            _camera.Update(new Vector3(-0.01214953f, 1.140012f, 2.391021f), new Vector3(-0.007645808f, 1.0049f, 1.400201f));

            var debugLight = new PointLight(new Vector3(0, 1.7f, -0.5f), Color4.White, 2);
            _scene.Add(debugLight);

            _scene.Add(new PointLight(new Vector3(5,5,5), Color4.White, 30));
            
            _scene.Add(new QuadLight(
                new Vector3(-0.5f, 1.99f, 0.5f),
                new Vector3(-0.5f, 1.99f,-0.5f),
                new Vector3(0.5f, 1.99f, -0.5f),
                new Vector3(0.5f, 1.99f,  0.5f),
                Color4.White
                ));

            var diffuse = new Material(MaterialType.Diffuse);
            //_scene.Add(Sphere.CreateOnGround(new Vector3(-0.5f, 0, 0), 0.3f, diffuse.WithColor(Color4.Green)));
            _scene.Add(new SphereLight(new Vector3(-0.5f, 0, 0), 0.3f, new Material(MaterialType.Light)));
            _scene.Add(Sphere.CreateOnGround(new Vector3(0.4f, 0, 0.6f), 0.2f, Material.Glass.WithColor(Color4.Red)));
            _scene.Add(Sphere.CreateOnGround(new Vector3(0.5f, 0, -0.4f), 0.4f, Material.Metal));

            var wallMat = new Material(MaterialType.Diffuse);
            //top
            _scene.Add(new Quad(
                new Vector3(-1, 2, -1),
                new Vector3(1, 2, -1),
                new Vector3(1, 2,  4),
                new Vector3(-1, 2, 4),
                 wallMat
                ));

            //bottom
            _scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 4),
                new Vector3(1, 0, 4),
                new Vector3(1, 0, -1),
                 wallMat
                ));

            //left
            _scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(-1, 2, -1),
                new Vector3(-1, 2, 4),
                new Vector3(-1, 0, 4),
                 wallMat
                ));

            //right
            _scene.Add(new Quad(
                new Vector3(1, 0, -1),
                new Vector3(1, 0, 4),
                new Vector3(1, 2, 4),
                new Vector3(1, 2, -1),
                 wallMat
                ));

            //back
            _scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                new Vector3(1, 2, -1),
                new Vector3(-1, 2, -1),
                 wallMat
                ));

            //front
            _scene.Add(new Quad(
                new Vector3(-1, 0, 4),
                new Vector3(-1, 2, 4),
                new Vector3(1, 2, 4),
                new Vector3(1, 0, 4),
                 wallMat
                ));
        }

        public void PathTracerTest()
        {
            _camera.Update(new Vector3(3.61508f, 2.465492f, 8.432084f), new Vector3(3.699683f, 2.259647f, 7.457158f));
            AddFloor();
            var mat = new Material(MaterialType.Diffuse);
            _scene.Add(new Sphere(new Vector3(0, 0.2f, 0), 0.2f, mat.WithColor(Color4.Red)));
            _scene.Add(new Sphere(new Vector3(1, 0.4f, 0), 0.4f, mat.WithColor(Color4.Blue)));
            _scene.Add(new Sphere(new Vector3(2.5f, 0.8f, 0), 0.8f, mat.WithColor(Color4.Green)));
            _scene.Add(new Sphere(new Vector3(5, 1.6f, 0), 1.6f, mat.WithColor(Color4.Yellow)));

            //facing down
            var quad = new Quad(
                new Vector3(1, 5f, 0),
                new Vector3(1, 5f, 1),
                new Vector3(0, 5f, 1),
                new Vector3(0, 5f, 0),
                    new Material(MaterialType.Diffuse).WithColor(Color4.Green)
                );

            //facing up
            //var quad = new Quad(
            //    new Vector3(0, 5f, 0),
            //    new Vector3(0, 5f, 1),
            //    new Vector3(1, 5f, 1),
            //    new Vector3(1, 5f, 0),
            //        new Material(MaterialType.Diffuse)
            //    );

            _scene.Add(new QuadLight(quad));
        }
    }
}
