using System.Collections.Generic;
using RayTracer.World.Objects;

namespace RayTracer.World
{
    public class Scene
    {
        public LightSource LightSource { get; set; }
        public List<Primitive> Objects { get; } = new List<Primitive>();  
    }
}
