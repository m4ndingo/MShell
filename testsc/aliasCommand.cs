using System;
using System.Collections.Generic;

namespace testsc
{
    class aliasCommand : CoreCommand
    {
        public Dictionary<string, string> aliases = new Dictionary<string, string>();
        public override void Run()
        {
            ConsoleWrite("Run(): aliasCommand : CoreCommand - Running: {0}", this.cmd_with_args);
            LoadAliases();
            if(isValidAlias(this.cmd_without_args).Equals(false))
            {
                ConsoleWrite("Run(): aliasCommand : CoreCommand - Invalid Alias");
                return;
            }

            this.results = aliases[this.cmd_without_args];
            this.result_type = RESULT_TYPE.COMMANDS;
        }
        private bool isValidAlias(string key)
        {
            return aliases.ContainsKey(key);
        }
        private void LoadAliases()
        {
            aliases.Clear();
            aliases.Add("q", "loop 0");
        }
    }
}