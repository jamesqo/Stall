using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public static class Cache
    {
        public static readonly string LocalAppData;
        public static readonly string Desktop;
        public static readonly string StartMenu;

        static Cache()
        {
            LocalAppData = Environment.GetFolderPath(Environment
                .SpecialFolder.LocalApplicationData);
            Desktop = Environment.GetFolderPath(Environment
                .SpecialFolder.Desktop);
            StartMenu = Environment.GetFolderPath(Environment
                .SpecialFolder.StartMenu);
        }
    }
}
