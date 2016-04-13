namespace RayTracer
{
    public static class Constants
    {
        public const float ShadowRayEpsilon = 0.0001f;
        public const float MinimumRayT = 0.0001f;

        public const int MaxRayBounces = 32;
        public const float RussianRouletteDieChance = 0.5f;
    }
}
