﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Shading.Tracers;
using RayTracer.Structures;
using RayTracer.World.Ambiance;
using RayTracer.World.Lighting;
using RayTracer.World.Objects;
using RayTracer.World.Objects.Complex;

namespace RayTracer.World
{
    public class Scene
    {
        private readonly IRayTracer _tracer;
        private readonly bool _constructBvh;
        public List<PointLight> PointLights { get; set; } = new List<PointLight>();

        public List<ISurfaceLight> SurfaceLights { get; set; } = new List<ISurfaceLight>();

        private readonly List<Intersectable> _intersectables = new List<Intersectable>();
        private readonly List<Boundable> _boundables = new List<Boundable>();

        public readonly List<Intersectable> Objects = new List<Intersectable>();

        private BoundingVolumeHierarchy BVH;

        public Skybox Skybox = new SingleColorSkybox(Color4.Black);

        public Scene(IRayTracer tracer, bool constructBVH)
        {
            _tracer = tracer;
            _constructBvh = constructBVH;
        }

        public void Add(PointLight pointLight)
        {
            PointLights.Add(pointLight);
        }

        public void Add(QuadLight quadLight)
        {
            SurfaceLights.Add(quadLight);
            Add((Intersectable)quadLight);
        }

        public void Add(SphereLight sphereLight)
        {
            SurfaceLights.Add(sphereLight);
            Add((Intersectable)sphereLight);
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

        public Color3 Sample(Ray ray, RNG random, bool ignoreLight)
        {
            return _tracer.Sample(this, ray, random, ignoreLight);
        }

        public void Construct()
        {
            Objects.Clear();
            Objects.AddRange(_intersectables);
            if (_constructBvh)
            {
                BVH = new BoundingVolumeHierarchy(_boundables);
                Objects.Add(BVH);
            }
            else
            {
                Objects.AddRange(_boundables);
            }
        }

        public void Clear()
        {
            SurfaceLights.Clear();
            PointLights.Clear();
            _intersectables.Clear();
            _boundables.Clear();
            Objects.Clear();
        }
    }
}
