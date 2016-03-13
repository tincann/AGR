using ObjParser;
using OpenTK;
using RayTracer.Lighting;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer
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
