using System.Collections.Generic;
using ObjParser;
using ObjParser.Types;
using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.World.Objects
{
    public class TriangleMesh : Intersectable
    {
        private readonly Obj _obj;

        public Vector3 Position { get; set; }

        public TriangleMesh(Vector3 position, Obj obj)
        {
            Position = position;
            _obj = obj;

            foreach (var face in _obj.FaceList)
            {
                var p1 = ToVector3(_obj.VertexList[face.VertexIndexList[0] - 1]) + Position;
                var p2 = ToVector3(_obj.VertexList[face.VertexIndexList[1] - 1]) + Position;
                var p3 = ToVector3(_obj.VertexList[face.VertexIndexList[2] - 1]) + Position;
                Triangles.Add(new Triangle(p1, p2, p3, MaterialType.Diffuse, Color4.Green));
            }
        }

        public bool Intersect(Ray ray, out Intersection intersection)
        {
            //todo intersect bounding box
            intersection = null;
            float closestDistance = float.MaxValue;
            foreach (var triangle in Triangles)
            {
                Intersection i;
                if (triangle.Intersect(ray, out i))
                {
                    if (i.Distance < closestDistance)
                    {
                        closestDistance = i.Distance;
                        intersection = i;
                    }
                }
            }
            if (intersection == null)
            {
                return false;
            }

            return true;
        }

        public List<Triangle> Triangles { get; } = new List<Triangle>();

        private static Vector3 ToVector3(Vertex vertex)
        {
            return new Vector3((float) vertex.X, (float) vertex.Y, (float) vertex.Z);
        }
    }
}
