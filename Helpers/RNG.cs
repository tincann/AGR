using System;
using OpenTK;

namespace RayTracer.Helpers
{
    public static class RNG
    {
        private static readonly Random _r = new Random();

        public static Vector3 RandomVector()
        {
            return new Vector3(
                RandomFloat() * 2 - 1, 
                RandomFloat() * 2 - 1, 
                RandomFloat() * 2 - 1);
        }

        public static Vector3 RandomVectorOnHemisphere(Vector3 orientation)
        {
            //calculate random point on hemisphere
            while (true)
            {
                var vec = RNG.RandomVector();
                if (vec.Length <= 1 && Vector3.Dot(vec, orientation) > 0)
                {
                    return vec;
                }
            }
        }

        public static float RandomFloat()
        {
            return (float)_r.NextDouble();
        }
    }
}
