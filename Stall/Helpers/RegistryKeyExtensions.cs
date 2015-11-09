using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal static class RegistryKeyExtensions
    {
        public static IEnumerable<string> RemoveApps(this RegistryKey baseKey, IEnumerable<string> appNames)
        {
            if (appNames.Count() == 0)
                return appNames;

            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            using (var parent = baseKey.OpenSubKey(Constants.UninstallKey))
            {
                if (parent == null) // Parent doesn't exist
                    return appNames;

                string[] keyNames = parent.GetSubKeyNames();

                foreach (string keyName in keyNames)
                {
                    string name, command;

                    using (var key = parent.OpenSubKey(keyName))
                    {
                        name = key.DisplayName();
                        command = key.UninstallString();
                    }

                    if (name != null && command != null)
                    {
                        if (!dict.ContainsKey(name)) // Trust the first one
                            dict.Add(name, command);
                    }
                }
            }

            var list = appNames.Distinct().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                string command;
                if (!dict.TryGetValue(list[i], out command))
                    continue;
                Process.Start(command).WaitForExit();
                list.RemoveAt(i--);
            }

            return list;
        }

        public static string DisplayName(this RegistryKey appKey)
            => ((string)appKey.GetValue("DisplayName"))?.Trim();

        public static string UninstallString(this RegistryKey appKey)
            => (string)(appKey.GetValue("QuietUninstallString") ?? appKey.GetValue("UninstallString"));
    }
}
