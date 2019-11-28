using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace testsc
{
    class uniqCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Get unique lines";
        }
        public override void Run()
        {
            bool ignoreCase = Core.readSetting("ignorecase", "0").Equals("1");  // set to 0 for case sensitive

            string output = string.Join("\n", last_message.Split('\n').Distinct(ignoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture).ToList());

            ConsoleWrite(output);
        }
    }
}
