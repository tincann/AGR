using ObjParser;
using OpenTK;
using RayTracer.World.Objects;

namespace RayTracer
{
    public static class ObjLoader
    {
        public static TriangleMesh Load(string path)
        {
            var obj = new Obj();
            obj.LoadObj(path);
            return new TriangleMesh(new Vector3(0,0, 50),  obj);
        }
    }
}
