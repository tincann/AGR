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
        public BVHNode Root;
        public BoundingVolumeHierarchy(List<Boundable> boundables)
        {
            var time = DateTime.UtcNow;
            Console.WriteLine($"Building BVH for {boundables.Count()} boundables...");
            Root = Construct(new BVHNode(boundables));
            Console.WriteLine($"Constuction done in {DateTime.UtcNow - time}");
        }
        
        public BVHNode Construct(BVHNode leaf)
        {
            //sort on x
            var xOrdered = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.X).ToList();
            //sort on y
            var yOrdered = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.Y).ToList();
            //sort on z
            var zOrdered = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.Z).ToList();

            var planeX = CalculateBestSplitPlane(xOrdered);
            var planeY = CalculateBestSplitPlane(yOrdered);
            var planeZ = CalculateBestSplitPlane(zOrdered);

            var bestPlane = new List<SplitPlane> {planeX, planeY, planeZ}.OrderBy(x => x.Cost).FirstOrDefault();
            
            //don't split further
            if (bestPlane.Cost > leaf.BoundingBox.Area*leaf.Boundables.Count)
            {
                return leaf;
            }

            var left = new BVHNode(bestPlane.Left);
            var right = new BVHNode(bestPlane.Right);

            var parent = new BVHNode(Construct(left), Construct(right));
            return parent;
        }
        
        private SplitPlane CalculateBestSplitPlane(List<Boundable> boundables)
        {
            var bestSplitPlane = new SplitPlane(float.MaxValue, null, null);
            for (int i = 1; i < boundables.Count; i++)
            {
                var left = boundables.Take(i).ToList();
                var leftB = BoundingBox.FromBoundables(left);
                var right = boundables.Skip(i).ToList();
                var rightB = BoundingBox.FromBoundables(right);

                var cost = leftB.Area * left.Count + rightB.Area * right.Count;
                if (cost < bestSplitPlane.Cost)
                {
                    bestSplitPlane = new SplitPlane(cost, left, right);
                }
            }

            return bestSplitPlane;
        }
    }

    struct SplitPlane
    {
        public readonly IEnumerable<Boundable> Left; 
        public readonly IEnumerable<Boundable> Right; 

        public SplitPlane(float cost, IEnumerable<Boundable> left, IEnumerable<Boundable> right)
        {
            Cost = cost;
            Left = left;
            Right = right;
        }
        public float Cost;

        public static SplitPlane GetBestSplitplane(params SplitPlane[] planes)
        {
            return planes.OrderBy(x => x.Cost).FirstOrDefault();
        }
    }

    public class BVHNode : Intersectable
    {
        public BoundingBox BoundingBox { get; }

        private readonly BVHNode _left;
        private readonly BVHNode _right;

        public readonly List<Boundable> Boundables;

        private bool IsLeaf => Boundables != null;

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

        public BVHNode(IEnumerable<Boundable> boundables)
        {
            Boundables = boundables.ToList();
            BoundingBox = BoundingBox.Combine(Boundables.Select(x => x.BoundingBox).ToArray());
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            if (IsLeaf)
            {
                intersection = IntersectionHelper.GetClosestIntersection(ray, Boundables);
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
