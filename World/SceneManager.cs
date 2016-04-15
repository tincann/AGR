using System;
using System.Collections.Generic;
using System.Configuration;

namespace RayTracer.World
{
    public class SceneManager
    {
        private readonly Camera _camera;
        private readonly Scene _scene;
        readonly List<Action<Camera, Scene>> _constructors = new List<Action<Camera, Scene>>();
        public int CurrentScene { get; private set; }

        public SceneManager(Camera camera, Scene scene)
        {
            _camera = camera;
            _scene = scene;
        }

        public void Add(Action<Camera, Scene> constructor)
        {
            _constructors.Add(constructor);
        }

        public void Init()
        {
            CurrentScene = 0;
            SetScene(0);
        }

        public void Previous()
        {
            if (CurrentScene == 0)
            {
                return;
            }
            SetScene(--CurrentScene);
        }

        public void Next()
        {
            if (CurrentScene == _constructors.Count - 1)
            {
                return;
            }
            SetScene(++CurrentScene);
        }

        private void SetScene(int i)
        {
            _scene.Clear();
            _constructors[i].Invoke(_camera, _scene);
            _scene.Construct();
        }
    }
}
