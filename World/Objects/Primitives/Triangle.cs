using OpenTK;
using RayTracer.Helpers;
using RayTracer.Shading;
using RayTracer.Structures;

namespace RayTracer.World.Objects.Primitives
{
    public class Triangle : Primitive, Boundable
    {
        private readonly Vector3 _e1;
        private readonly Vector3 _e2;
        private readonly Vector3 _p1;
        public readonly Vector3 Normal;

        public BoundingBox BoundingBox { get; }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3, Material material)
            : base(material)
        {
            _p1 = p1;

            //vectors of triangle edges
            _e1 = Vector3.Subtract(p2, _p1);
            _e2 = Vector3.Subtract(p3, _p1);
            Normal = Vector3.Cross(_e1, _e2).Normalized();
            BoundingBox = BoundingBox.FromVectors(p1, p2, p3);
        }

        //https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
        public override bool Intersect(Ray ray, out Intersection intersection)
        {
            Statistics.Add("Triangle test");
            
            intersection = null;

            //don't intersect backside
            if (Vector3.Dot(ray.Direction, Normal) > 0)
            {
                return false;
            }

            var P = Vector3.Cross(ray.Direction, _e2);

            var det = Vector3.Dot(_e1, P);

            //if determinant is near zero, then there is no intersection
            if (det > -float.Epsilon && det < float.Epsilon)
            {
                return false;
            }

            var invDet = 1/det;

            var T = Vector3.Subtract(ray.Origin, _p1);

            //calculate u coordinate of triangle
            var u = Vector3.Dot(T, P)*invDet;

            //if it lies outside the triangle
            if (u < 0 || u > 1)
            {
                return false;
            }

            var Q = Vector3.Cross(T, _e1);

            //calculate v coordinate of triangle
            var v = Vector3.Dot(ray.Direction, Q)*invDet;

            //if it lies outside the triangle
            if (v < 0 || u + v > 1)
            {
                return false;
            }

            //t parameter of ray
            float t = Vector3.Dot(_e2, Q)*invDet;

            if (t > Constants.MinimumRayT && t <= ray.T)
            {
                var intersectionPoint = ray.GetPoint(t);
                ray.SetLength(t);
                intersection = new Intersection(this, ray, Normal, intersectionPoint, t, Material);
                Statistics.Add("Triangle test success");
                return true;
            }

            return false;
        }
    }
}