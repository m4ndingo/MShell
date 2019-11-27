using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace testsc
{
    class replaceCommand : CoreCommand
    {
        public replaceCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {
            KeyValuePair<string, string> kArgs = Core.getCmdArgs(args);
            Regex myRegex = new Regex(kArgs.Key, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string output = myRegex.Replace(last_message, kArgs.Value);            

            ConsoleWrite(output);
        }
        public override string Help(params string[] help_args)
        {
            return "Replace lines using regular expressions";
        }
    }
}