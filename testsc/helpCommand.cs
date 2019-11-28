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
                string help = command.Value.Help(command.Key); // call command help
                if (properties!=null && properties.help != null)
                    help = properties.help + ". " + help;

                ConsoleWrite(
                    "{0,-12}{1,-8}{2,-1}{3,-2} {4}",
                    command.Key,
                    properties != null ? properties.input_type.ToString() : "",
                    properties != null ? properties.is_setting ? "S" : " " : "",
                    properties != null ? properties.is_alias   ? "A" : " " : "",
                    help);
            }
        }
        public override string Help(params string[] help_args)
        {
            return "I'm the help. Try: \"help\" or \"help [command]\"";
        }
    }
}