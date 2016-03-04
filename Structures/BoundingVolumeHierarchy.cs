using System;
using System.Collections.Generic;
using System.Linq;
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
        public BoundingBox BoundingBox { get; }

        private readonly BVHNode _left;
        private readonly BVHNode _right;

        private readonly List<Boundable> _boundables;

        private bool IsLeaf => _boundables != null;

        public BVHNode(BVHNode left)
        {
            _left = left;
            BoundingBox = left.BoundingBox;
        }

        public BVHNode(BVHNode left, BVHNode right)
        {
            _left = left;
            _right = right;
            BoundingBox = BoundingBox.Combine(left.BoundingBox, right.BoundingBox);
        }

        public BVHNode(List<Boundable> boundables)
        {
            _boundables = boundables;
            BoundingBox = BoundingBox.Combine(boundables.Select(x => x.BoundingBox).ToArray());
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            if (IsLeaf)
            {
                intersection = IntersectionHelper.GetClosestIntersection(ray, _boundables);
                return intersection != null;
            }

            if (BoundingBox.Intersect(ray))
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

            intersection = null;
            return false;
        }
    }
}
