using System.Collections.Generic;
using System.Linq;

namespace testsc
{
    class sortCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Sort lines";
        }
        public override void Run()
        {
            List<string> lines = last_message.Split('\n').ToList();
            lines.Sort();
            ConsoleWrite_Atom(string.Join("\n", lines));
        }
    }
}