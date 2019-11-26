using System;
using System.Collections.Generic;

namespace testsc
{
    class aliasCommand : CoreCommand
    {        
        public override void Run()
        {
            //ConsoleWrite("Run(): aliasCommand : CoreCommand - Running: {0}", this.cmd_with_args);
            LoadAliases();
            if(isValidAlias(this.cmd_without_args).Equals(false))
            {
                if (this.cmd_without_args.Equals("alias"))
                {
                    DumpAliases();
                }
                else
                {
                    ConsoleWrite("Run(): aliasCommand : CoreCommand - Invalid Alias '{0}'", this.cmd_without_args);
                }
                return;
            }

            this.results = Core.aliases[this.cmd_without_args];
            this.result_type = RESULT_TYPE.COMMANDS;
        }

        private void DumpAliases()
        {
            if (args.Length > 0)
            {
                if(Core.aliases.ContainsKey(args).Equals(false))
                {
                    ConsoleWrite("DumpAliases(): Alias {0} not found", args);
                    return;
                }
                ConsoleWrite("{0};{1}", args, Core.aliases[args]);
                return;
            }
            foreach (KeyValuePair<string, string> alias in Core.aliases)
            {
                ConsoleWrite("{0};{1}", alias.Key, alias.Value);
            }
        }

        private bool isValidAlias(string key)
        {
            return Core.aliases.ContainsKey(key);
        }
        private void LoadAliases()
        {
            Core.aliases.Clear();
            Core.aliases.Add("?", "help");
            Core.aliases.Add("q", "loop 0");
        }
        public override string Help(params string[] help_args)
        {
            string name = help_args[0];
            if (name.Equals("alias"))
            {
                return "I'm the alias command. Try: alias or alias [name]";
            }
            if (Core.aliases.Count.Equals(0))
                LoadAliases();
            return string.Format("alias '{0}' value '{1}'", name, Core.aliases[name]);
        }
    }
}