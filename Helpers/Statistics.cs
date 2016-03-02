using System.Collections.Generic;

namespace RayTracer.Helpers
{
    public static class Statistics
    {
        public readonly static Dictionary<string, int> Stats = new Dictionary<string, int>();
        private static object _lock = new object();
        public static void Add(string key)
        {
            return;
            lock (_lock)
            {


                if (Stats.ContainsKey(key))
                {
                    Stats[key]++;
                }
                else
                {
                    Stats.Add(key, 1);
                }
            }
        }

        public static int Get(string key)
        {
            if (Stats.ContainsKey(key))
            {
                return Stats[key];
            }

            return 0;
        }

        public static void Reset()
        {
            Stats.Clear();
        }
    }
}
