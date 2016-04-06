using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Tracers;
using RayTracer.Structures;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        private readonly IRayTracer _tracer;
        public List<LightSource> LightSources { get; set; } = new List<LightSource>();

        private readonly List<Intersectable> _intersectables = new List<Intersectable>();
        private readonly List<Boundable> _boundables = new List<Boundable>();

        public readonly List<Intersectable> Objects = new List<Intersectable>(); 

        public BoundingVolumeHierarchy BVH;

        public readonly Skybox Skybox = new Skybox(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\assets\\skybox3.jpg"));

        public Scene(IRayTracer tracer)
        {
            _tracer = tracer;
        }

        public void Add(LightSource lightSource)
        {
            LightSources.Add(lightSource);
        }

        public void Add(Intersectable intersectable)
        {
            _intersectables.Add(intersectable);
        }

        public void Add(Boundable boundable)
        {
            _boundables.Add(boundable);
        }

        public void Add(TriangleMesh mesh)
        {
            _boundables.AddRange(mesh.Triangles);
        }

        public Color3 Sample(Ray ray)
        {
            return _tracer.Sample(this, ray);
        }

        public void Construct()
        {
            Objects.Clear();
            BVH = new BoundingVolumeHierarchy(_boundables);
            Objects.AddRange(_intersectables);
            Objects.Add(BVH.Root);
        }
        
    }
}
