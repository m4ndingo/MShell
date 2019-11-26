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
                    ConsoleWrite("'{0}' help: {1} {2}", args, Core.core_commands[args].Help(args), Core.core_commands[args].isPipe ? "isPipe: True" : "");
                }else
                {
                    ConsoleWrite("helpCommand: unknown command '{0}'", args);
                }
                return;
            }
            foreach (KeyValuePair<string, CoreCommand> command in Core.core_commands)
            {
                ConsoleWrite("{0}\t{1}{2}", command.Key, command.Value.Help(command.Key), command.Value.isPipe ? " (pipe)" : "");
            }
        }
        public override string Help(params string[] help_args)
        {
            return "I'm the help. Try: \"help\" or \"help [command]\"";
        }
    }
}