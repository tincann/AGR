using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.World.Objects
{
    public interface Intersectable
    {
        bool Intersect(Ray ray, out Intersection intersection);
    }
}
