using System.Collections.Generic;

namespace testsc
{
    class helpCommand : CoreCommand
    {
        public override void Run()
        {            
            if (args.Length > 0) 
            {
                if (Core.isValidCommand(args))
                {
                    ConsoleWrite("'{0}' help:\n{1}", args, Core.core_commands[args].Help(args));
                }else
                {
                    ConsoleWrite("helpCommand: unknown command '{0}'", args);
                }
                return;
            }
            foreach (KeyValuePair<string, CoreCommand> alias in Core.core_commands)
            {
                ConsoleWrite("{0}\t{1}", alias.Key, alias.Value.Help(alias.Key));
            }
        }
        public override string Help(params string[] help_args)
        {
            return "I'm the help. Try: \"help\" or \"help [command]\"";
        }
    }
}