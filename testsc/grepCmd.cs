using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace testsc
{
    class grepCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Grep lines using regex. Settings: ignorecase escapeargs";
        }
        public override void Run()
        {
            bool ignoreCase = Core.readSetting("ignorecase", "0").Equals("1");  // set to 0 for case sensitive
            bool escapeArgs = Core.readSetting("escapeargs", "0").Equals("1");  // escape regex
            try
            {
                Regex myRegex = new Regex(escapeArgs ? Regex.Escape(args) : args,
                    (ignoreCase ? RegexOptions.IgnoreCase : 0) | RegexOptions.Compiled);
                List<string> resultList = last_message.Split('\n').ToList<string>().Where(line => myRegex.IsMatch(line)).ToList();

                ConsoleWrite(string.Join("\n", resultList));
            }catch(Exception ex)
            {
                ConsoleWrite("grepCommand:Run(): {0}", ex.Message);
            }
        }
    }
}