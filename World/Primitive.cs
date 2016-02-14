using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RayTracer.World
{
    public abstract class Primitive
    {
        Vector3 Position { get; set; }
        Material Material { get; set; }

        public abstract bool Intersect(Ray ray);
    }
}
