using System;
using OpenTK;

namespace RayTracer.Helpers
{
    public class RNG
    {
        private Random _r;

        public static RNG[] CreateMultipleRNGs(int count)
        {
            var m = new Random();
            var r = new RNG[count];
            for (int i = 0; i < count; i++)
            {
                r[i] = new RNG(m.Next());
            }
            return r;
        }

        public RNG(int seed)
        {
            _r = new Random(seed);
        }

        public RNG()
        {
            _r = new Random();
        }
        
        public Vector3 RandomVector()
        {
            return new Vector3(
                RandomFloat() * 2 - 1, 
                RandomFloat() * 2 - 1, 
                RandomFloat() * 2 - 1);
        }

        public bool TestChance(float chance)
        {
            return RandomFloat() <= chance;
        }

        public Vector3 RandomVectorOnHemisphere(Vector3 orientation)
        {
            //calculate random point on hemisphere
            while (true)
            {
                var vec = RandomVector();
                if (vec.LengthFast <= 1)
                {
                    if (Vector3.Dot(vec, orientation) < 0)
                    {
                        return -vec;
                    }
                    return vec;
                }
            }
        }

        public Vector3 CosineDistributed(Vector3 normal)
        {
            //generate random cosine distributed vector
            float r0 = RandomFloat(), r1 = RandomFloat();
            float r = (float)Math.Sqrt(r0);
            float theta = 2 * (float)Math.PI * r1;
            float x = r * (float)Math.Cos(theta);
            float y = r * (float)Math.Sin(theta);
            var z = (float)Math.Sqrt(1 - r0);
            var vec = new Vector3(x, y, z);
            
            //transform to world space
            var w = new Vector3(1, 0, 0);
            if (Math.Abs(normal.X) > 0.99)
            {
                w = new Vector3(0, 1, 0);
            }

            var t = Vector3.Cross(normal, w);
            var b = Vector3.Cross(t, normal);

            return vec.X * t + vec.Y * b + vec.Z * normal;
        }

        public float RandomFloat()
        {
            return (float)_r.NextDouble();
        }
    }
}
