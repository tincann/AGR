using System.Collections.Concurrent;

namespace RayTracer.Helpers
{
    public class Statistics
    {
        private static readonly ConcurrentDictionary<string, int> Stats = new ConcurrentDictionary<string, int>();
        public static bool Enabled = true;

        public static void Add(string key)
        {
            if (Enabled)
            {
                Stats.AddOrUpdate(key, 1, (k, v) => v + 1);
            }
        }

        public static int Get(string key)
        {
            int value;
            Stats.TryGetValue(key, out value);
            return value;
        }

        public static void Reset()
        {
            Stats.Clear();
        }
    }
}