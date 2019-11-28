using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace testsc
{
    class replaceCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Replace lines using regular expressions. Settings: ignorecase";
        }
        public override void Run()
        {
            bool ignoreCase = Core.readSetting("ignorecase", "0").Equals("1");  // set to 0 for case sensitive
            KeyValuePair<string, string> kArgs = Core.getCmdArgs(args);
            Regex myRegex = new Regex(kArgs.Key, (ignoreCase ? RegexOptions.IgnoreCase : 0) | RegexOptions.Compiled);
            
            string output = myRegex.Replace(last_message, Core.UnescapeArgs(kArgs.Value));            

            ConsoleWrite(output);
        }
    }
}