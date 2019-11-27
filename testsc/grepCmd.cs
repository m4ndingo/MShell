﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            bool ignoreCase = Core.readSetting("ignorecase", "1").Equals("1");
            bool escapeArgs = Core.readSetting("escapeargs", "0").Equals("1");
            try
            {
                Regex myRegex = new Regex(escapeArgs ? Regex.Escape(args) : args, (ignoreCase ? RegexOptions.IgnoreCase : 0) | RegexOptions.Compiled);
                List<string> resultList = last_message.Split('\n').ToList<string>().Where(line => myRegex.IsMatch(line)).ToList();

                ConsoleWrite(string.Join("\n", resultList));
            }catch(Exception ex)
            {
                ConsoleWrite("grepCommand:Run(): {0}", ex.Message);
            }
        }
        public override string Help(params string[] help_args)
        {
            return "Grep lines";
        }
    }
}