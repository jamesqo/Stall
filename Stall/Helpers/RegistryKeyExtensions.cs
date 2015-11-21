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
        public static IEnumerable<string> HideApps(this RegistryKey baseKey, IEnumerable<string> appNames)
        {
            if (appNames.Count() == 0)
                return appNames;

            var comparer = StringComparer.InvariantCultureIgnoreCase;
            using (var parent = baseKey.OpenSubKey(Constants.UninstallKey))
            {
                if (parent == null) // Parent doesn't exist
                    return appNames;

                // Maps app names to their full registry paths
                var found = new Dictionary<string, string>();

                parent.ForEachKey(key =>
                {
                    string appName = key.DisplayName();
                    if (appNames.Contains(appName, comparer))
                        found[appName] = key.Name;
                });

                // Remove the registry entries
                foreach (string keyName in found.Values)
                {
                    // Avoid UAC dialogs by using Process.Start
                    var info = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        FileName = "reg",
                        Arguments = $@"delete ""{keyName}"" /f"
                    };
                    Process.Start(info);
                }

                return appNames.Except(found.Keys, comparer);
            }
        }

        public static IEnumerable<string> RemoveApps(this RegistryKey baseKey, IEnumerable<string> appNames)
        {
            if (appNames.Count() == 0)
                return appNames;

            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            using (var parent = baseKey.OpenSubKey(Constants.UninstallKey))
            {
                if (parent == null) // Parent doesn't exist
                    return appNames;

                parent.ForEachKey(key =>
                {
                    string name = key.DisplayName();
                    string command = key.UninstallString();
                    if (name != null && command != null)
                    {
                        if (!dict.ContainsKey(name)) // Trust the first one
                            dict.Add(name, command);
                    }
                });
            }

            var list = appNames.Distinct().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                string command;
                if (!dict.TryGetValue(list[i], out command))
                    continue;
                var info = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = command,
                    UseShellExecute = false
                };
                Process.Start(info).WaitForExit();
                list.RemoveAt(i--);
            }

            return list;
        }

        public static void ForEachKey(this RegistryKey key, Action<RegistryKey> action)
        {
            foreach (string name in key.GetSubKeyNames())
            {
                using (var child = key.OpenSubKey(name))
                    action(child);
            }
        }

        public static string DisplayName(this RegistryKey appKey)
            => ((string)appKey.GetValue("DisplayName"))?.Trim();

        public static string UninstallString(this RegistryKey appKey)
            => (string)(appKey.GetValue("QuietUninstallString") ?? appKey.GetValue("UninstallString"));
    }
}
