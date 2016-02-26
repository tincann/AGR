using RayTracer.World.Objects;

namespace RayTracer.Structures
{
    // ReSharper disable once InconsistentNaming
    public interface Boundable : Intersectable
    {
        BoundingBox BoundingBox { get; }
    }
}
