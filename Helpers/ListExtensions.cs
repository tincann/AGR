using System;
using System.Collections.Generic;

namespace RayTracer.Helpers
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list, Random r)
        {
            return list[r.Next(list.Count)];
        }
    }
}
