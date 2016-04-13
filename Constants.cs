namespace RayTracer
{
    public static class Constants
    {
        public const float ShadowRayEpsilon = 0.0001f;
        public const float MinimumRayT = 0.0001f;

        public const int MaxRayBounces = 16;
        public const float RussianRouletteDieChance = 0.4f;
    }
}
