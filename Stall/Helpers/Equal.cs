using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class Equal
    {
        public static bool Paths(string left, string right)
        {
            left = Path.GetFullPath(left).TrimEnd('\\');
            right = Path.GetFullPath(right).TrimEnd('\\');

            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }
    }
}
