using Microsoft.Win32;
using Stall.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    class Program
    {
        static int Main(string[] args) =>
            OptMain(Options.Parse(args));

        static int OptMain(Options opts)
        {
            var status = opts.Status;
            if (status != null)
                return RaiseError(status.Code, status.Message ?? Constants.Usage);

            switch (opts.IntendedUsage)
            {
                case Usage.Remove:
                    return RemoveMain(opts);
                case Usage.Install:
                    return InstallMain(opts);
                case Usage.Hide:
                    return HideMain(opts);
                default:
                    throw new NotImplementedException("Not implemented yet.");
            }
        }

        static int InstallMain(Options opts)
        {
            if (opts.Target == null)
                return RaiseError(Errors.NoTargets, Constants.Usage);

            string root = Path.Combine(Cache.LocalAppData, opts.AppName);
            var directory = new DirectoryInfo(root);

            if (directory.Exists)
            {
                if (!opts.Overwrite)
                    return RaiseError(Errors.ExistingFolder, $"Directory {root} already exists! Exiting...");

                // rm -rf that directory
                Directory.Delete(root, true); // TODO: This fails when the app is still running?
                // TODO: What if the user specifies a folder in AppData\Local?
            }

            // Copy the folder into AppData
            Folder.RecursiveCopy(opts.Target.FullName, root);

            if (opts.IsScript | opts.AddToPath)
                directory.AddToPath(); // $PATH env var

            if (!opts.IsScript)
            {
                int error = ShortcutMain(opts);
                if (error != 0)
                    return error;

                return RegMain(opts);
            }

            return 0;
        }

        static int ShortcutMain(Options opts)
        {
            string root = Path.Combine(Cache.LocalAppData, opts.AppName);

            string programPath = opts.ProgramPath;
            var status = Normalize.SpecialPath(root, ref programPath);
            if (status != null)
                return RaiseError(status);

            string iconPath = opts.IconPath;
            if (iconPath != null)
            {
                status = Normalize.SpecialPath(root, ref iconPath);
                if (status != null)
                    return RaiseError(status);
            }

            string contents = @"[InternetShortcut]
URL=file:///" + programPath.Replace('\\', '/'); // TODO: Use $ when VS gets its highlighting straight.

            if (iconPath != null)
            {
                contents += $@"
IconIndex=0
IconFile={iconPath.Replace('\\', '/')}";
            }

            foreach (string folder in GetShortcutFolders(opts))
            {
                string file = Path.Combine(folder, $"{opts.AppName}.url");
                File.WriteAllText(file, contents);
            }

            return 0;
        }

        static int RegMain(Options opts)
        {
            string root = Path.Combine(Cache.LocalAppData, opts.AppName);
            var directory = new DirectoryInfo(root);

            // Create an uninstall.cmd script
            string keyName = $@"{Constants.UninstallKey}\{opts.AppName}";
            
            // Generate the commands to remove the shortcuts
            var shortcuts = GetShortcutFolders(opts).Select(f => Path.Combine(f, $"{opts.AppName}.url"));
            var deletions = shortcuts.Select(s => $@"del ""{s}""");
            string joined = string.Join(Environment.NewLine, deletions);

            string script = $@"@echo off
setlocal
cd %~dp0\..
:: Delete the shortcuts
{joined}
:: Use start to finish before we're deleted
start /min """" cmd /c ^
%= Use reg to delete the app key =% ^
reg delete ""HKCU\{keyName}"" /f ^& ^
%= Remove the app's root folder =% ^
rd /s /q ""{opts.AppName}""";
            string scriptPath = Path.Combine(root, "uninstall.cmd");
            File.WriteAllText(scriptPath, script);

            // Create the uninstaller Registry key
            if (!opts.Overwrite)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(keyName))
                {
                    if (key != null) // OpenSubKey returns null if the key doesn't exist
                        return RaiseError(Errors.ExistingKey, $"Registry key {key} already exists! Exiting...");
                }
            }
            using (var key = Registry.CurrentUser.CreateSubKey(keyName))
            {
                // strings
                string path = opts.IconPath;
                if (path != null)
                {
                    // TODO: Support using the executable as an icon here, if it has one.
                    var status = Normalize.SpecialPath(root, ref path);
                    if (status != null)
                        return RaiseError(status);
                    key.SetValue("DisplayIcon", path);
                }

                key.SetValue("DisplayName", opts.AppName);
                if (opts.Version != null)
                    key.SetValue("DisplayVersion", opts.Version);
                key.SetValue("InstallDate", DateTime.Now.ToString("yyyymmdd"));
                key.SetValue("InstallLocation", root);
                if (opts.Publisher != null)
                    key.SetValue("Publisher", opts.Publisher);
                key.SetValue("QuietUninstallString", scriptPath);
                key.SetValue("UninstallString", scriptPath); // TODO: Show GIF of some sort?
                if (opts.ProjectUri != null)
                    key.SetValue("URLInfoAbout", opts.ProjectUri);
                if (opts.ReleasesUri != null)
                    key.SetValue("URLUpdateInfo", opts.ReleasesUri);

                // dwords
                key.SetValue("EstimatedSize", new DirectoryInfo(root).Size());
                key.SetValue("NoModify", 1);
                key.SetValue("NoRepair", 1);
                key.SetValue("Language", Constants.EnglishLocale);
            }

            return 0;
        }

        static int HideMain(Options opts)
        {
            var appsPresent = opts.ToRemove.AsEnumerable();
            foreach (var key in GetUninstallBaseKeys())
            {
                appsPresent = key.HideApps(appsPresent);

                if (appsPresent.Count() == 0)
                    return 0;
            }

            // Couldn't hide all the apps
            string joined = string.Join(", ", appsPresent);
            return RaiseError(Errors.RemoveFailed, $"Unable to hide these apps: {joined}");
        }

        static int RemoveMain(Options opts)
        {
            var appsPresent = opts.ToRemove.AsEnumerable();
            foreach (var key in GetUninstallBaseKeys())
            {
                appsPresent = key.RemoveApps(appsPresent);

                if (appsPresent.Count() == 0)
                    return 0;
            }

            // Couldn't remove all the apps
            string joined = string.Join(", ", appsPresent);
            return RaiseError(Errors.RemoveFailed, $"Unable to remove these apps: {joined}");
        }

        static IEnumerable<RegistryKey> GetUninstallBaseKeys()
        {
            yield return RegistryKey.OpenBaseKey(RegistryHive
                .CurrentUser, RegistryView.Registry64);

            yield return RegistryKey.OpenBaseKey(RegistryHive
                .LocalMachine, RegistryView.Registry64);

            if (Environment.Is64BitOperatingSystem)
            {
                // Open WOW64 keys
                yield return RegistryKey.OpenBaseKey(RegistryHive
                    .CurrentUser, RegistryView.Registry32);

                yield return RegistryKey.OpenBaseKey(RegistryHive
                    .LocalMachine, RegistryView.Registry32);
            }
        }

        static IEnumerable<string> GetShortcutFolders(Options opts)
        {
            // TODO: Add options to en/disable certain folders.
            // Use [Flags] functionality similar to how Squirrel does it.
            yield return Cache.Desktop;
            yield return Path.Combine(Cache.StartMenu, "Programs");
        }

        static int RaiseError(Status status) =>
            RaiseError(status.Code, status.Message);

        static int RaiseError(int code, string message)
        {
            Console.Error.WriteLine(message);
            return code;
        }
    }
}
