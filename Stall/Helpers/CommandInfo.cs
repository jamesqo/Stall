using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall.Helpers
{
    internal class CommandInfo
    {
        public CommandInfo(string file, string arguments)
        {
            this.FileName = file;
            this.Arguments = arguments;
        }

        public string FileName { get; }
        public string Arguments { get; }

        public static CommandInfo Parse(string command)
        {
            int index = GetArgumentIndex(command);

            if (index == -1) // No arguments
                return new CommandInfo(command, null);

            string file = command.Substring(0, index).Trim();
            string arguments = command.Substring(index);
            return new CommandInfo(file, arguments);
        }

        private static int GetArgumentIndex(string command)
        {
            bool quoted = false;

            for (int i = 0; i < command.Length; i++)
            {
                char c = command[i];
                if (c == '"')
                    quoted = !quoted;
                else if (char.IsWhiteSpace(c) && !quoted)
                {
                    while (char.IsWhiteSpace(command[i]))
                        i++;
                    return i;
                }
            }

            return -1;
        }
    }
}
