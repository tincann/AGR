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
using RayTracer.World.Objects.Complex;

namespace RayTracer.World
{
    public class Scene
    {
        private readonly IRayTracer _tracer;
        public List<PointLight> PointLights { get; set; } = new List<PointLight>();
        public List<SurfaceLight> SurfaceLights { get; set; } = new List<SurfaceLight>();

        private readonly List<Intersectable> _intersectables = new List<Intersectable>();
        private readonly List<Boundable> _boundables = new List<Boundable>();

        public readonly List<Intersectable> Objects = new List<Intersectable>(); 

        public BoundingVolumeHierarchy BVH;

        public readonly Skybox Skybox = new Skybox(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\assets\\skybox3.jpg"));

        public Scene(IRayTracer tracer)
        {
            _tracer = tracer;
        }

        public void Add(PointLight pointLight)
        {
            PointLights.Add(pointLight);
        }

        public void Add(SurfaceLight surfaceLight)
        {
            SurfaceLights.Add(surfaceLight);
        }

        public void Add(Intersectable intersectable)
        {
            _intersectables.Add(intersectable);
        }

        public void Add(Boundable boundable)
        {
            _boundables.Add(boundable);
        }

        public void Add(IMesh mesh)
        {
            _boundables.AddRange(mesh.Boundables);
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
