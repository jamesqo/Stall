using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public interface IOptions
    {
        Status Status { get; }
        bool IsScript { get; }
        bool Overwrite { get; }
        string AppName { get; }
        bool AddToPath { get; }
        string IconPath { get; }
        Version Version { get; }
        string Publisher { get; }
        string ProjectUri { get; }
        string ReleasesUri { get; }
        string ProgramPath { get; }
        Usage IntendedUsage { get; }
        DirectoryInfo Target { get; }
        UnwantedApps ToRemove { get; }
    }
}
