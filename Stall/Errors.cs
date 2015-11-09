using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public static class Errors
    {
        public const int ShowHelp = 0;
        public const int NoArgs = ShowHelp + 1;
        public const int CutShort = NoArgs + 1;
        public const int BadPath = CutShort + 1;
        public const int BadVersion = BadPath + 1;
        public const int ExistingFolder = BadVersion + 1;
        public const int ExistingKey = ExistingFolder + 1;
        public const int RemoveFailed = ExistingKey + 1;
        public const int NoTargets = RemoveFailed + 1;
        public const int TwoDirectories = NoTargets + 1;
        public const int MissingArgs = TwoDirectories + 1;
        public const int BadAbsolute = MissingArgs + 1;
    }
}
