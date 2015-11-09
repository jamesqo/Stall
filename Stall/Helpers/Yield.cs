using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class Yield
    {
        public static IEnumerable<T> Wrap<T>(IEnumerable<T> seq)
        {
            foreach (var item in seq)
                yield return item;
        }
    }
}
