using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.Lighting
{
    public interface Texture
    {
        Color3 GetColor(Vector3 point);
    }
}
