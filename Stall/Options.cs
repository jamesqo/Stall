using Stall.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public class Options : IOptions
    {
        internal Options(IOptions other)
        {
            this.Status = other.Status;
            this.Target = other.Target;
            this.Version = other.Version;
            this.AppName = other.AppName;
            this.IconPath = other.IconPath;
            this.IsScript = other.IsScript;
            this.ToRemove = other.ToRemove;
            this.AddToPath = other.AddToPath;
            this.Overwrite = other.Overwrite;
            this.Publisher = other.Publisher;
            this.ProjectUri = other.ProjectUri;
            this.ProgramPath = other.ProgramPath;
            this.ReleasesUri = other.ReleasesUri;
            this.IntendedUsage = other.IntendedUsage;
        }

        public static Options Parse(IEnumerable<string> args) =>
            new OptionsBuilder().Apply(args).ToOptions();

        public Status Status { get; }
        public bool IsScript { get; }
        public bool Overwrite { get; }
        public string AppName { get; }
        public bool AddToPath { get; }
        public string IconPath { get; }
        public Version Version { get; }
        public string Publisher { get; }
        public string ProjectUri { get; }
        public string ReleasesUri { get; }
        public string ProgramPath { get; }
        public Usage IntendedUsage { get; }
        public DirectoryInfo Target { get; }
        public UnwantedApps ToRemove { get; }
    }
}
