using System.Collections.Generic;

namespace testsc
{
    class varCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Pipe data to @variable or read with \"var @variable\"";
        }
        public override void Run()
        {
            if (args.Length > 0 && last_message.Length > 0)
                Core.variables[args] = last_message;
            else if (Core.variables.ContainsKey(args))
                ConsoleWrite(Core.variables[args]);
            else if (args.Length > 0)
                ConsoleWrite("varCommand: Run(): Variable \"{0}\" not found", args);
            else
                DumpVars();
        }
        private void DumpVars()
        {
            foreach (KeyValuePair<string, string> variable in Core.variables)
            {
                string[] lines = variable.Value.Split('\n');
                ConsoleWrite("{0};{1};{2}", variable.Key, lines.Length.ToString(), lines.Length > 1 ? "[list]" : variable.Value);
            }
        }
    }
}