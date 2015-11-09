using Stall.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public class OptionsBuilder : IOptions
    {
        private readonly List<string> toRemove;

        public OptionsBuilder()
        {
            toRemove = new List<string>();
        }

        public Status Status { get; set; }
        public bool IsScript { get; set; }
        public bool Overwrite { get; set; }
        public string AppName { get; set; }
        public bool AddToPath { get; set; }
        public string IconPath { get; set; }
        public Version Version { get; set; }
        public string Publisher { get; set; }
        public string ProjectUri { get; set; }
        public string ReleasesUri { get; set; }
        public string ProgramPath { get; set; }
        public Usage IntendedUsage { get; set; }
        public DirectoryInfo Target { get; set; }
        public UnwantedApps ToRemove => new UnwantedApps(toRemove);

        public OptionsBuilder BlackList(string appName)
        {
            toRemove.Add(appName);
            return this;
        }

        public OptionsBuilder WithStatus(int code, string message = null) 
            => WithStatus(new Status(code, message));

        public OptionsBuilder WithStatus(Status status)
        {
            Status = status;
            return this;
        }

        public Options ToOptions() => new Options(this);
    }
}
