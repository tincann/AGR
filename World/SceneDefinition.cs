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
    public static class SceneDefinitions
    {
        

        private static void AddSkybox(Scene scene)
        {
            scene.Skybox = new TexturedSkybox(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\assets\\skybox3.jpg"));
        }

        private static void AddFloor(Scene scene)
        {
            scene.Add(new Plane(
                Vector3.UnitY,
                0,
                new Material(MaterialType.Diffuse) { Texture = new Checkerboard(5, Color4.Bisque, Color4.White) }
            ));
        }

        public static void AddLight(Scene scene)
        {
            scene.Add(new PointLight(new Vector3(5, 6, 3), Color4.White, 20));
            scene.Add(new PointLight(new Vector3(-2000, 2000, 2000), Color4.White, 7000000));
        }

        public static void Teapot(Camera camera, Scene scene)
        {
            AddSkybox(scene);
            AddFloor(scene);
            AddLight(scene);
            var teapot = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\teapot.obj", Vector3.Zero, Material.Metal);
            scene.Add(teapot);
        }

        public static void Default(Camera camera, Scene scene)
        {
            camera.Update(new Vector3(1.922451f, 2.341791f, 3.731561f), new Vector3(1.634576f, 1.94541f, 2.859776f));
            AddSkybox(scene);
            AddFloor(scene);
            AddLight(scene);
            
            //var cube = ObjLoader.Load("C:\\Users\\Morten\\Documents\\Visual Studio 2015\\Projects\\AGR\\Meshes\\cube.obj",
            //    new Vector3(3, 0.51f, 1),
            //    Material.Glass);
            //_scene.Add(cube);
            
            scene.Add(new Sphere(new Vector3(1, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Red, Specularity = 0.9f }));
            scene.Add(new Sphere(new Vector3(2, 0.5f, -1), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Green, Specularity = 0.9f }));
            scene.Add(new Sphere(new Vector3(1.5f, 0.5f, -1.9f), 0.5f,
                new Material(MaterialType.Diffuse) { Color = Color4.Blue, Specularity = 0.9f }));
            scene.Add(new Sphere(new Vector3(1.5f, 1.25f, -1.4f), 0.5f,
                new Material(MaterialType.Diffuse) { Specularity = 0.95f }));
            
            scene.Add(new Sphere(new Vector3(0, 1, 2), 1, Material.Glass.WithColor(Color4.Red)));
            scene.Add(new Sphere(new Vector3(0, 3, 2), 1, Material.Metal));
        }

        public static void BeerTest(Camera camera, Scene scene)
        {
            AddSkybox(scene);
            camera.Update(new Vector3(3.61508f, 2.465492f, 8.432084f), new Vector3(3.699683f, 2.259647f, 7.457158f));
            AddFloor(scene);
            AddLight(scene);

            var mat = Material.Glass;
            mat.Color = Color4.Red;
            
            scene.Add(new Sphere(new Vector3(0, 0.2f, 0), 0.2f, mat));
            scene.Add(new Sphere(new Vector3(1, 0.4f, 0), 0.4f, mat));
            scene.Add(new Sphere(new Vector3(2.5f, 0.8f, 0), 0.8f, mat));
            scene.Add(new Sphere(new Vector3(5, 1.6f, 0), 1.6f, mat));
        }

        public static void PathTracerBox(Camera camera, Scene scene)
        {
            //AddSkybox();
            camera.Update(new Vector3(-0.01214953f, 1.140012f, 2.391021f), new Vector3(-0.007645808f, 1.0049f, 1.400201f));

            var debugLight = new PointLight(new Vector3(0, 1.7f, -0.5f), Color4.White, 2);
            scene.Add(debugLight);

            scene.Add(new PointLight(new Vector3(5,5,5), Color4.White, 30));


            var lightWidth = 0.75f;
            var hw = lightWidth/2;
            scene.Add(new SurfaceLight(
                new Vector3(-hw, 1.99f, hw),
                new Vector3(-hw, 1.99f,-hw),
                new Vector3(hw, 1.99f, -hw),
                new Vector3(hw, 1.99f,  hw),
                Color4.White
                ));

            var diffuse = new Material(MaterialType.Diffuse);
            scene.Add(Sphere.CreateOnGround(new Vector3(-0.5f, 0, 0), 0.3f, diffuse.WithColor(Color4.Green)));
            //_scene.Add(Sphere.CreateOnGround(new Vector3(-0.5f, 0, 0), 0.3f, new Material(MaterialType.Light)));
            scene.Add(Sphere.CreateOnGround(new Vector3(0.4f, 0, 0.6f), 0.2f, Material.Metal));
            scene.Add(Sphere.CreateOnGround(new Vector3(0.5f, 0, -0.4f), 0.4f, Material.Glass));

            var wallMat = new Material(MaterialType.Diffuse) { Color = new Color3(0.7f, 0.7f, 0.7f)};
            //top
            scene.Add(new Quad(
                new Vector3(-1, 2, -1),
                new Vector3(1, 2, -1),
                new Vector3(1, 2,  4),
                new Vector3(-1, 2, 4),
                 wallMat
                ));

            //bottom
            scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 4),
                new Vector3(1, 0, 4),
                new Vector3(1, 0, -1),
                 wallMat
                ));

            //left
            scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(-1, 2, -1),
                new Vector3(-1, 2, 4),
                new Vector3(-1, 0, 4),
                 wallMat
                ));

            //right
            scene.Add(new Quad(
                new Vector3(1, 0, -1),
                new Vector3(1, 0, 4),
                new Vector3(1, 2, 4),
                new Vector3(1, 2, -1),
                 wallMat
                ));

            //back
            scene.Add(new Quad(
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                new Vector3(1, 2, -1),
                new Vector3(-1, 2, -1),
                 wallMat
                ));

            //front
            scene.Add(new Quad(
                new Vector3(-1, 0, 4),
                new Vector3(-1, 2, 4),
                new Vector3(1, 2, 4),
                new Vector3(1, 0, 4),
                 wallMat
                ));
        }

        public static void DarkRoom(Camera camera, Scene scene)
        {
            camera.Update(new Vector3(3.61508f, 2.465492f, 8.432084f), new Vector3(3.699683f, 2.259647f, 7.457158f));

            scene.Skybox = new SingleColorSkybox(Color4.Black);

            //light
            scene.Add(CreateLight(new Vector3(10,7.5f,-10), Color4.White, 10, 5));

            //floor
            scene.Add(new Plane(
                Vector3.UnitY,
                0,
                new Material(MaterialType.Diffuse) { Texture = new Checkerboard(5, Color4.DarkGray, Color4.WhiteSmoke) }
            ));
            
            var mat = new Material(MaterialType.Diffuse);
            scene.Add(Sphere.CreateOnGround(new Vector3(0,0,0), 0.5f, mat.WithColor(Color4.Red)));
            scene.Add(Sphere.CreateOnGround(new Vector3(1,0,1), 0.5f, mat.WithColor(Color4.Green)));
            scene.Add(Sphere.CreateOnGround(new Vector3(2,0,2), 0.5f, mat.WithColor(Color4.Blue)));
            //_scene.Add(new Sphere(new Vector3(0, 0.2f, 0), 0.2f, mat.WithColor(Color4.Red)));
            //_scene.Add(new Sphere(new Vector3(1, 0.4f, 0), 0.4f, mat.WithColor(Color4.Blue)));
            //_scene.Add(new Sphere(new Vector3(2.5f, 0.8f, 0), 0.8f, mat.WithColor(Color4.Green)));
            //_scene.Add(new Sphere(new Vector3(5, 1.6f, 0), 1.6f, mat.WithColor(Color4.Yellow)));

        }

        private static SurfaceLight CreateLight(Vector3 position, Color4 color, float width, float height)
        {
            float hWidth = width/2;
            float hHeight = height/2;
            //facing down
            return new SurfaceLight(
                new Vector3(hWidth, 0, -hHeight) + position,
                new Vector3(hWidth, 0, hHeight) + position,
                new Vector3(-hWidth, 0, hHeight) + position,
                new Vector3(-hWidth, 0, -hHeight) + position,
                color
            );
        }
    }
}
