using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class EnumerableExtensions
    {
        public static T[] AsArray<T>(this IEnumerable<T> seq)
        {
            if (seq is T[])
                return (T[])seq;
            return seq.ToArray();
        }

        public static List<T> AsList<T>(this IEnumerable<T> seq)
        {
            if (seq is List<T>)
                return (List<T>)seq;
            return seq.ToList();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> seq, T value)
        {
            foreach (var item in seq)
                yield return item;
            yield return value;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> seq, params T[] array)
        {
            foreach (var item in seq)
                yield return item;
            foreach (var item in array)
                yield return item;
        }
    }
}
