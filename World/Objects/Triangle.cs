using OpenTK;
using OpenTK.Graphics;
using RayTracer.Lighting;

namespace RayTracer.World.Objects
{
    public class Triangle : Primitive
    {
        private readonly Vector3 _e1;
        private readonly Vector3 _e2;
        private readonly Vector3 _p1;
        private readonly Vector3 _normal;

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3, MaterialType materialType, Color3 color)
            : base(materialType, color)
        {
            _p1 = p1;

            //vectors of triangle edges
            _e1 = Vector3.Subtract(p2, _p1);
            _e2 = Vector3.Subtract(p3, _p1);
            _normal = Vector3.Cross(_e1, _e2).Normalized();
        }

        //https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
        public override bool Intersect(Ray ray, out Intersection intersection)
        {
            intersection = null;

            //don't intersect with primitive that ray came from
            if (this.Equals(ray.OriginPrimitive))
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

            if (t > 0.001)
            {
                var intersectionPoint = ray.Origin + t*ray.Direction;
                intersection = new Intersection(this, ray, _normal, intersectionPoint, t, MaterialType, Color);
                return true;
            }

            return false;
        }
    }
}