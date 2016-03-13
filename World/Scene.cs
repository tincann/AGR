using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenTK.Graphics;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Structures;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        public List<LightSource> LightSources { get; set; } = new List<LightSource>();

        private readonly List<Intersectable> _intersectables = new List<Intersectable>();
        private readonly List<Boundable> _boundables = new List<Boundable>();

        public readonly List<Intersectable> Objects = new List<Intersectable>(); 

        private BoundingVolumeHierarchy _bvh;

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

        public void Construct()
        {
            Objects.Clear();
            _bvh = new BoundingVolumeHierarchy(_boundables);
            Objects.AddRange(_intersectables);
            Objects.Add(_bvh.Root);
        }

        public Color3 Intersect(Ray ray)
        {
            Debug.Assert(_bvh != null);

            if (ray.BouncesLeft < 1)
            {
                return new Color3(Color4.Red);
            }

            //get nearest intersection
            var intersection = IntersectionHelper.GetClosestIntersection(ray, Objects);
            if (intersection == null)
            {
                return new Color3(Color4.Black);
            }

            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Diffuse:
                    return LightingModel.DirectIllumination(this, intersection);
                case MaterialType.Specular:
                    return LightingModel.Specular(this, intersection);
                case MaterialType.Dielectric:
                    return LightingModel.Dielectric(this, intersection);
            }
            throw new NotImplementedException();
        }
    }
}
