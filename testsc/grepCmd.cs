using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testsc
{
    class grepCommand : CoreCommand
    {
        public grepCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {
            Regex myRegex = new Regex(args, RegexOptions.IgnoreCase|RegexOptions.Compiled);
            List<string> resultList = last_message.Split('\n').ToList<string>().Where(line=>myRegex.IsMatch(line)).ToList();

            ConsoleWrite(string.Join("\n", resultList));
        }
        public override string Help(params string[] help_args)
        {
            return "Grep lines";
        }
    }
}