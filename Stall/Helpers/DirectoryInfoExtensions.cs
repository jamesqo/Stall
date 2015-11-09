using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class DirectoryInfoExtensions
    {
        public static bool AddToPath(this DirectoryInfo directory)
        {
            string path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            path = path.TrimEnd(';');

            // Check if already present
            string[] paths = path.Split(';');
            for (int i = 0; i < paths.Length; i++)
            {
                string sub = paths[i];
                sub = Environment.ExpandEnvironmentVariables(sub);
                if (Equal.Paths(sub, directory.FullName))
                    return false;
            }

            // Add it
            path += $";{directory.FullName}";
            return true;
        }

        public static IEnumerable<FileInfo> EnumerateAllFiles(this DirectoryInfo directory)
        {
            foreach (var file in directory.EnumerateFiles())
                yield return file;

            foreach (var sub in directory.EnumerateDirectories())
            {
                foreach (var file in sub.EnumerateAllFiles())
                    yield return file;
            }
        }

        public static int Size(this DirectoryInfo directory)
        {
            long bytes = 0;

            foreach (var file in directory.EnumerateAllFiles())
                bytes += file.Length;

            return (int)(bytes / 1024); // we want kilos
        }
    }
}
