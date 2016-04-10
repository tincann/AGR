using System;
using System.Collections.Generic;

namespace RayTracer.Helpers
{
    public static class ListExtensions
    {
        static readonly Random R = new Random();
        public static T GetRandom<T>(this List<T> list)
        {
            return list[R.Next(list.Count)];
        }
    }
}
