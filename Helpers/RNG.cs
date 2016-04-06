using System;

namespace RayTracer.Helpers
{
    public static class RNG
    {
        private static readonly Random _r = new Random();

        public static float RandomFloat()
        {
            return (float)_r.NextDouble();
        }
    }
}
