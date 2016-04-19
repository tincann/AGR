using System;
using System.Collections.Generic;
using System.Linq;
using RayTracer.Helpers;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer.Structures
{
    public class BoundingVolumeHierarchy : Intersectable
    {
        public readonly BVHNode Root;
        private int _logNumBoundables;
        public BoundingVolumeHierarchy(List<Boundable> boundables)
        {
            _logNumBoundables = (int)Math.Ceiling(Math.Log(boundables.Count));
            var time = DateTime.UtcNow;
            Console.WriteLine($"Building BVH for {boundables.Count} boundables...");
            Root = Construct(new BVHNode(boundables));
            Console.WriteLine($"Constuction done in {DateTime.UtcNow - time}");
        }
        
        public BVHNode Construct(BVHNode leaf)
        {
            if (leaf.Boundables.Count == 0)
            {
                return leaf;
            }

            var bb = BoundingBox.FromBoundables(leaf.Boundables);
            var xLen = bb.Max.X - bb.Min.X;
            var yLen = bb.Max.Y - bb.Min.Y;
            var zLen = bb.Max.Z - bb.Min.Z;

            List<Boundable> sorted;
            if (xLen > yLen && xLen > zLen)
            {
                sorted = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.X).ToList();
            }
            else if (yLen > xLen && yLen > zLen)
            {
                sorted = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.Y).ToList();
            }
            else
            {
                sorted = leaf.Boundables.OrderBy(x => x.BoundingBox.Centroid.Z).ToList();
            }
            
            var bestPlane = CalculateBestSplitPlane(sorted);
            
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

        
        public bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = null;
            var nodes = new Stack<BVHNode>(_logNumBoundables);
            nodes.Push(Root);
            while (nodes.Count > 0)
            {
                var node = nodes.Pop();
                if (node.IsLeaf)
                {
                    var i = IntersectionHelper.GetClosestIntersection(ray, node.Boundables);
                    intersection = IntersectionHelper.GetMinimumIntersection(intersection, i);
                }

                Intersection i1 = null, i2 = null;
                float t1 = float.MaxValue, t2 = float.MaxValue;
                var goLeft = node._left != null && node._left.BoundingBox.Intersect(ray, out t1);
                var goRight = node._right != null && node._right.BoundingBox.Intersect(ray, out t2);

                if (goLeft && goRight)
                {
                    //choose shortest
                    if (t1 < t2)
                    {
                        nodes.Push(node._right);
                        nodes.Push(node._left);
                    }
                    else
                    {
                        nodes.Push(node._left);
                        nodes.Push(node._right);
                    }
                }
                else if (goLeft)
                {
                    nodes.Push(node._left);
                }
                else if (goRight)
                {
                    nodes.Push(node._right);
                }
            }
            
            return intersection != null;
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

    public class BVHNode
    {
        public BoundingBox BoundingBox { get; }

        public readonly BVHNode _left;
        public readonly BVHNode _right;

        public readonly List<Boundable> Boundables;

        public bool IsLeaf => Boundables != null;

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
    }
}
