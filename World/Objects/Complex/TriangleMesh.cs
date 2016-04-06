using System.Collections.Generic;
using ObjParser;
using ObjParser.Types;
using OpenTK;
using RayTracer.Shading;
using RayTracer.Structures;
using RayTracer.World.Objects.Primitives;

namespace RayTracer.World.Objects.Complex
{
    public class TriangleMesh : IMesh
    {
        private readonly Obj _obj;

        public Vector3 Position { get; set; }

        public TriangleMesh(Vector3 position, Obj obj, Material material)
        {
            Position = position;
            _obj = obj;

            foreach (var face in _obj.FaceList)
            {
                var p1 = ToVector3(_obj.VertexList[face.VertexIndexList[0] - 1]) + Position;
                var p2 = ToVector3(_obj.VertexList[face.VertexIndexList[1] - 1]) + Position;
                var p3 = ToVector3(_obj.VertexList[face.VertexIndexList[2] - 1]) + Position;
                Boundables.Add(new Triangle(p1, p2, p3, material));
            }
        }

        public List<Boundable> Boundables { get; } = new List<Boundable>();

        private static Vector3 ToVector3(Vertex vertex)
        {
            return new Vector3((float) vertex.X, (float) vertex.Y, (float) vertex.Z);
        }
    }
}
