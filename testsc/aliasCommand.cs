using System.Collections.Generic;
using System.Linq;

namespace testsc
{
    class aliasCommand : CoreCommand
    {
        public aliasCommand()
        {
            if (Core.aliases.Count.Equals(0))
                LoadAliases();
        }
        public override void Run()
        {
            //ConsoleWrite("Run(): aliasCommand : CoreCommand - Running: {0}", this.cmd_with_args);
            if(isValidAlias(this.cmd_without_args).Equals(false))
            {
                if (this.cmd_without_args.Equals("alias"))
                {
                    if (this.args.Contains(" "))
                        AddAlias();
                    else
                        DumpAliases();
                }
                else
                {
                    ConsoleWrite("Run(): aliasCommand : CoreCommand - Invalid Alias '{0}'", this.cmd_without_args);
                }
                this.result_type = RESULT_TYPE.NONE;
                return;
            }

            this.results = Core.aliases[this.cmd_without_args];
            this.result_type = RESULT_TYPE.COMMANDS;
        }

        private void AddAlias()
        {
            string[] args = this.args.Split(' ');
            string alias = args[0];
            string commands = string.Join(" ", args.Skip(1));
            Core.aliases[alias] = commands;
            Core.core_commands[alias] = Core.aliasManager;
        }

        private void DumpAliases()
        {
            if (args.Length > 0)
            {
                if(Core.aliases.ContainsKey(args).Equals(false))
                {
                    ConsoleWrite("DumpAliases(): Alias \"{0}\" not found", args);
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
                return "I'm the alias command. Try: alias or alias [name] or alias [newalias] [commands]";
            }
            if (Core.aliases.Count.Equals(0))
                LoadAliases();
            return string.Format("Alias '{0}' current value '{1}'", name, Core.aliases[name]);
        }
    }
}