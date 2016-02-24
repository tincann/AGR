using ObjParser;
using OpenTK;
using RayTracer.Lighting;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer
{
    public static class ObjLoader
    {
        public static TriangleMesh Load(string path, Material material)
        {
            var obj = new Obj();
            obj.LoadObj(path);
            return new TriangleMesh(new Vector3(0, 0.5f, 0),  obj, material);
        }
    }
}
