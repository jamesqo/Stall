using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class Normalize
    {
        public static string Path(string path)
        {
            path = path.Replace('/', '\\').TrimEnd('\\');
            return System.IO.Path.GetFullPath(path);
        }

        public static Status SpecialPath(string relative, ref string path)
            => SpecialPath(relative, path, out path);

        public static Status SpecialPath(string relative, string path, out string normal)
        {
            normal = null;
            if (path[0] != ':') // Special symbol to indicate absoluteness; see Constants.Usage for more details
            {
                if (System.IO.Path.IsPathRooted(path))
                    return new Status(Errors.BadAbsolute, "Prefix paths with : when using absolute paths. For more details, run stall --help.");

                path = System.IO.Path.Combine(relative, path);
            }
            else
                path = path.Substring(1);

            path = Path(path);

            if (!File.Exists(path))
                return new Status(Errors.BadPath, $"The file {path} does not exist.");

            normal = path;
            return default(Status); // Use null to indicate success
        }

        public static string Uri(string uri)
        {
            if (uri.IndexOf("://") == -1)
            {
                uri = "http://" + uri;
            }
            return uri;
        }
    }
}
