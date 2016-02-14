using System.Collections.Generic;

namespace RayTracer.World
{
    public class Scene
    {
        public LightSource LightSource { get; set; }
        public List<Primitive> Objects { get; set; } 
    }
}
