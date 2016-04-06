using System.Collections.Generic;
using RayTracer.Structures;

namespace RayTracer.World.Objects.Complex
{
    public interface IMesh
    {
        List<Boundable> Boundables { get; }
    }
}