﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;
using RayTracer.Lighting;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        public List<LightSource> LightSources { get; set; } = new List<LightSource>();
        public List<Intersectable> Objects { get; } = new List<Intersectable>();

        public Color3 Intersect(Ray ray)
        {
            //get nearest intersection
            var intersection = GetNearestIntersection(ray);
            if (intersection == null)
            {
                return new Color3(Color4.Black);
            }

            switch (intersection.Material.MaterialType)
            {
                case MaterialType.Diffuse:
                    return LightingModel.DirectIlumination(this, intersection, LightSources.First());
                case MaterialType.Mirror:
                    return LightingModel.Reflection(this, intersection);
                case MaterialType.Specular:
                    throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

       

        public bool DoesIntersect(Ray ray)
        {
            foreach(var obj in Objects)
            {
                Intersection intersection;
                if (obj.Intersect(ray, out intersection))
                {
                    return true;
                }
            }
            return false;
        }

        private Intersection GetNearestIntersection(Ray ray)
        {
            float closestDistance = float.MaxValue;
            Intersection closestIntersection = null;
            foreach (var obj in Objects)
            {
                Intersection intersection;
                if (obj.Intersect(ray, out intersection))
                {
                    if (intersection.Distance < closestDistance)
                    {
                        closestDistance = intersection.Distance;
                        closestIntersection = intersection;
                    }
                }
            }
            
            return closestIntersection;
        }
    }
}
