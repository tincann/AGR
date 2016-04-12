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

        public Vector3 RandomVectorOnHemisphere(Vector3 orientation)
        {
            //calculate random point on hemisphere
            while (true)
            {
                var vec = RandomVector();
                if (vec.LengthFast <= 1 && Vector3.Dot(vec, orientation) > 0)
                {
                    return vec.Normalized();
                }
            }
        }

        public float RandomFloat()
        {
            return (float)_r.NextDouble();
        }
    }
}
