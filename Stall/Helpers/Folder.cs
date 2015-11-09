using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class Folder
    {
        public static void RecursiveCopy(string src, string dest) =>
            RecursiveCopy(new DirectoryInfo(src), new DirectoryInfo(dest));

        private static void RecursiveCopy(DirectoryInfo src, DirectoryInfo dest)
        {
            dest.Create();
            foreach (var sub in src.EnumerateDirectories())
                RecursiveCopy(sub, new DirectoryInfo(Path.Combine(dest.FullName, sub.Name)));
            foreach (var file in src.EnumerateFiles())
                file.CopyTo(Path.Combine(dest.FullName, file.Name));
        }
    }
}
