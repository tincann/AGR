using System.Collections.Generic;
using OpenTK;
using RayTracer.Shading;
using RayTracer.Structures;
using RayTracer.World.Objects.Primitives;

namespace RayTracer.World.Objects.Complex
{
    public class Quad : IMesh
    {
        public readonly Vector3 P1;
        public readonly Vector3 P2;
        public readonly Vector3 P3;
        public readonly Vector3 P4;

        public readonly Vector3 Width;
        public readonly Vector3 Depth;
        public Material Material { get; set; }

        public readonly Vector3 Normal;

        public List<Boundable> Boundables { get; } = new List<Boundable>();

        public Quad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Material material)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            Material = material;
            var t1 = new Triangle(p1, p2, p4, material);
            var t2 = new Triangle(p2, p3, p4, material);
            Normal = t1.Normal;
            Boundables.Add(t1);
            Boundables.Add(t2);

            Width = p2 - p1;
            Depth = p4 - p1;
        }
    }
}
