﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace testsc
{
    class aliasCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            string name = help_args[0];
            if (name.Equals("alias"))
            {
                return "I'm the alias command. Try: alias or alias [name] or alias [newalias] [commands]";
            }
            return string.Format("Alias '{0}' current value '{1}'", name, Core.aliases[name]);
        }
        public override void Run()
        {
            if(isValidAlias(this.cmd_without_args).Equals(false))
            {
                if (this.cmd_without_args.Equals("alias"))
                {
                    if (this.args.Contains(" "))
                        AddAlias();
                    else if (last_message.Length > 0) 
                    {
                        AddMultipleAlias();
                    }else
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

        private void AddMultipleAlias()
        {
            foreach(string line in last_message.Split('\n'))
            {
                string[] args = line.Split(';');
                if (args.Length < 2) continue;
                AddAlias(args[0], args[1]);
            }
        }

        private void AddAlias()
        {
            string[] args = this.args.Split(' ');
            string alias = args[0];
            string commands = string.Join(" ", args.Skip(1));
            AddAlias(alias, commands);
        }

        public void AddAlias(string name, string commands, INPUT_TYPE input_type = INPUT_TYPE.PARAMS)
        {
            // save to shared array (static objects share vars)
            Core.RegisterNewProperty(name, new Core.CommandProperty { input_type = input_type }); 

            Core.aliases[name] = commands;                  // save new alias
            Core.core_commands[name] = this;                // this is manager :)
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
    }
}