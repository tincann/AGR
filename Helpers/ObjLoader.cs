using ObjParser;
using OpenTK;
using RayTracer.Shading;
using RayTracer.World.Objects;
using RayTracer.World.Objects.Complex;

namespace RayTracer.Helpers
{
    public static class ObjLoader
    {
        public static TriangleMesh Load(string path, Vector3 position, Material material)
        {
            var obj = new Obj();
            obj.LoadObj(path);
            return new TriangleMesh(position,  obj, material);
        }
    }
}
