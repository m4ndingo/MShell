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

            KeyValuePair<string, string> kArgs = Core.getCmdArgs(args);
            string sRegex = kArgs.Key;
            Regex matchRegex = null;
            try
            {
                matchRegex = new Regex(sRegex, (ignoreCase ? RegexOptions.IgnoreCase : 0) | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
            }
            catch (Exception ex)
            {
                ConsoleWrite_Atom("matchCommand: Run(): {0}", ex.Message);
            }

            if (matchRegex == null)
                return;

            try
            {
                foreach (string line in last_message.Split('\n'))
                {
                MatchCollection match = matchRegex.Matches(line);
                    for (int i = 0; i < match.Count; i++)
                    {
                        string output = "";
                        if (kArgs.Value.Equals(""))
                            output = match[i].Groups[0].ToString();
                        else
                        {
                            string[] items = new string[match[i].Groups.Count - 1];
                            for (int n = 1; n < match[i].Groups.Count; n++)
                            {
                                items[n - 1] = match[i].Groups[n].ToString();
                            }
                            output = string.Format(kArgs.Value.Replace("\\;", ";"), items);
                        }
                        ConsoleWrite("match", output);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrite_Atom("matchCommand: Run(): {0}", ex.Message);
                return;
            }

        }
    }
}
