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
        private List<BVHNode> Nodes; 
        public BVHNode Construct(List<Boundable> boundables)
        {
            //sort on x
            var xOrdered = boundables.OrderBy(x => x.BoundingBox.Centroid.X).ToList();
            //sort on y
            var yOrdered = boundables.OrderBy(x => x.BoundingBox.Centroid.Y).ToList();
            //sort on z
            var zOrdered = boundables.OrderBy(x => x.BoundingBox.Centroid.Z).ToList();


            throw new NotImplementedException();
        }

        private SplitPlane CalculateBestSplitPlane(List<Boundable> boundables)
        {
            var bestSplitPlane = new SplitPlane(float.MaxValue, -1);
            for (int i = 1; i < boundables.Count; i++)
            {
                var leftB = BoundingBox.FromBoundables(boundables.Take(i));
                var rightB = BoundingBox.FromBoundables(boundables.Skip(i));

                var cost = leftB.Area*i + rightB.Area*(boundables.Count - i);
                if (cost < bestSplitPlane.Cost)
                {
                    bestSplitPlane = new SplitPlane(cost, i);
                }
            }

            return bestSplitPlane;
        }
    }

    struct SplitPlane
    {
        public SplitPlane(float cost, int index)
        {
            Cost = cost;
            Index = index;
        }
        public float Cost;
        public int Index;
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
