using Stall.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stall.Helpers.New;

namespace Stall
{
    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder Apply(this OptionsBuilder builder, IEnumerable<string> args)
        {
            if (args.Count() == 0)
                return builder.WithStatus(Errors.NoArgs);

            string programPath = null;
            string iconPath = null;

            var array = SplitByEquals(args).ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                switch (array[i].ToLower())
                {
                    case "-?":
                    case "-h":
                    case "--help":
                        return builder.WithStatus(Errors.ShowHelp);
                    case "--add-path":
                        builder.AddToPath = true;
                        break;
                    case "-e":
                    case "--executable":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        programPath = array[i]; // Will be normalized + checked later
                        break;
                    case "-i":
                    case "--icon":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        iconPath = array[i]; // Will be normalized + checked later
                        break;
                    case "-n":
                    case "--name":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        var name = new StringBuilder(array[i]);
                        foreach (char bad in Path.GetInvalidFileNameChars())
                            name.Replace(bad, '_');
                        builder.AppName = name.ToString();
                        break;
                    case "--overwrite":
                        builder.Overwrite = true;
                        break;
                    case "--project-url":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        builder.ProjectUri = Normalize.Uri(array[i]);
                        break;
                    case "-p":
                    case "--publisher":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        builder.Publisher = array[i];
                        break;
                    case "--releases-url":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        builder.ReleasesUri = Normalize.Uri(array[i]);
                        break;
                    case "-s":
                    case "--script":
                        builder.IsScript = true;
                        break;
                    case "-un":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        builder.IntendedUsage = Usage.Remove;
                        foreach (string app in array[i].Split(','))
                            builder.BlackList(app.Trim());
                        break;
                    case "-v":
                    case "--version":
                        if (++i == array.Length)
                            return builder.WithStatus(Errors.CutShort);
                        string input = array[i];
                        Version version;
                        if (!Version.TryParse(input, out version))
                            return builder.WithStatus(Errors.BadVersion, $"{input} is not a valid version.");
                        builder.Version = version;
                        break;
                    default:
                        if (builder.Target != null)
                            return builder.WithStatus(Errors.TwoDirectories, "More than one directory was specified.");
                        string path = Normalize.Path(array[i]);
                        var directory = new DirectoryInfo(path);
                        if (!directory.Exists)
                            return builder.WithStatus(Errors.BadPath, $"The directory {path} does not exist.");
                        builder.Target = directory;
                        break;
                }
            }

            if (builder.IntendedUsage == Usage.Install)
            {
                if (builder.Target == null)
                    return builder.WithStatus(Errors.MissingArgs, "Please specify a target directory. For more details, run stall --help.");
                if (programPath == null)
                    return builder.WithStatus(Errors.MissingArgs, "Please specify the path to your app's executable with -e. For more details, run stall --help.");

                // Check ProgramPath and IconPath to make sure they exist
                string unused; // We don't want the normalized path yet
                string target = builder.Target.FullName;
                var status = Normalize.SpecialPath(target, programPath, out unused);
                if (status != null)
                    return builder.WithStatus(status);
                builder.ProgramPath = programPath;

                // Check IconPath if specified
                if (iconPath != null)
                {
                    status = Normalize.SpecialPath(target, iconPath, out unused);
                    if (status != null)
                        return builder.WithStatus(status);
                    builder.IconPath = iconPath;
                }

                // Try to infer the AppName if not present
                if (builder.AppName == null)
                    builder.AppName = builder.Target.Name;
            }

            return builder;
        }

        // TODO: This OK for apps with = signs?
        private static IEnumerable<string> SplitByEquals(IEnumerable<string> args)
        {
            foreach (string arg in args)
            {
                if (!arg.Contains("="))
                    yield return arg;
                else
                {
                    foreach (string subs in arg.Split('='))
                        yield return subs;
                }
            }
        }
    }
}
