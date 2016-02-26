using System;
using System.Collections.Generic;
using RayTracer.Helpers;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer.Structures
{
    public class BoundingVolumeHierarchy
    {
        public BVHNode Construct(List<Boundable> boundables)
        {
            throw new NotImplementedException();
        }
    }

    public class BVHNode : Intersectable
    {
        private readonly BoundingBox _boundingBox;

        private readonly Intersectable _left;
        private readonly Intersectable _right;

        public BVHNode(Boundable left)
        {
            _left = left;
            _boundingBox = left.BoundingBox;
        }

        public BVHNode(Boundable left, Boundable right)
        {
            _left = left;
            _right = right;
            _boundingBox = BoundingBox.Combine(left.BoundingBox, right.BoundingBox);
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = null;
            if(_boundingBox.Intersect(ray))
            {
                Intersection i1 = null, i2 = null;
                if (_left != null)
                {
                    _left.Intersect(ray, out i1);
                }
                if (_right != null)
                {
                    _right.Intersect(ray, out i2);
                }

                intersection = IntersectionHelper.GetMinimumIntersection(i1, i2);
                return intersection != null;
            }

            return false;
        }
    }
}
