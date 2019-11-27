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
            List<string> resultList = last_message.Split('\n').ToList<string>().Select(line => myRegex.Replace(line,string.Join(" ",kArgs.Value))).ToList();

            ConsoleWrite(string.Join("\n", resultList));
        }
        public override string Help(params string[] help_args)
        {
            return "Replace lines using regular expressions";
        }
    }
}