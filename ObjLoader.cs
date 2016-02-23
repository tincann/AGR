using ObjParser;
using OpenTK;
using RayTracer.Lighting;
using RayTracer.World;
using RayTracer.World.Objects;

namespace RayTracer
{
    public static class ObjLoader
    {
        public static TriangleMesh Load(string path, MaterialType materialType, Color3 color)
        {
            var obj = new Obj();
            obj.LoadObj(path);
            return new TriangleMesh(new Vector3(0,0, 0),  obj, materialType, color);
        }
    }
}
