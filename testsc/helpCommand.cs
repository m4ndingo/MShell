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
                Core.CommandProperty properties = Core.getCommandProperties(command.Key);
                ConsoleWrite(
                    "{0,-12}{3,-8} {1}{2}",
                    command.Key,
                    command.Value.Help(command.Key),
                    properties.input_type.Equals(CoreCommand.INPUT_TYPE.PIPE) ? " (pipe)" : "",
                    properties != null ? properties.input_type.ToString() : "");
            }
        }
        public override string Help(params string[] help_args)
        {
            return "I'm the help. Try: \"help\" or \"help [command]\"";
        }
    }
}