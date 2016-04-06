namespace RayTracer.World.Objects
{
    // ReSharper disable once InconsistentNaming
    public interface Intersectable
    {
        bool Intersect(Ray ray, out Intersection intersection);
    }
}
