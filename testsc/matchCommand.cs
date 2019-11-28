using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace testsc
{
    class matchCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Match data using regular expresions";
        }
        public override void Run()
        {
            bool ignoreCase = Core.readSetting("ignorecase", "0").Equals("1");  // set to 0 for case sensitive

            Regex matchRegex = null;
            try
            {
                matchRegex = new Regex("(" + args + ")", (ignoreCase ? RegexOptions.IgnoreCase : 0) | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
            }
            catch (Exception ex)
            {
                ConsoleWrite("matchCommand: Run(): {0}", ex.Message);
            }

            if (matchRegex == null)
                return;

            MatchCollection match = matchRegex.Matches(last_message);
            for (int i = 0; i < match.Count; i++)
            {
                ConsoleWrite(match[i].Groups[1].ToString());
                /*
                if (match[i].Groups.Count > 1)
                {
                    for (int n = 1; n < match[i].Groups.Count; n++)
                        ConsoleWrite(match[i].Groups[n].ToString());
                }*/
            }            
        }
    }
}
