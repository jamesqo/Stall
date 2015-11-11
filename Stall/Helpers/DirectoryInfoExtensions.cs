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
            // TODO: This runs even if the dir is already in the machine-wide $PATH.
            string path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

            // Check if already present
            foreach (string sub in path.Split(';'))
            {
                if (Equal.Paths(sub, directory.FullName))
                    return false;
            }

            // Add it
            string addend = directory.FullName;
            if (!path.EndsWith(";"))
                addend = ';' + addend;
            path += addend;
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.User);
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
