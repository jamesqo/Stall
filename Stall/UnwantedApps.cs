using Stall.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public class UnwantedApps : ReadOnlyCollection<string>
    {
        public UnwantedApps(IEnumerable<string> seq)
            : base(seq.AsList())
        { }
    }
}
