using System.Collections.Generic;
using System.IO;

namespace testsc
{
    class teeCommand : CoreCommand
    {
        public teeCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {
            string filename = args;
            bool append = false;
            bool force = false;
            if (filename.Length.Equals(0))
            {
                ConsoleWrite($"teeCommand: Run(): output file required");
                return;
            }                
            if (filename.Contains("-f"))
            {
                filename = filename.Replace("-f", "");
                force = true;
            }
            if (filename.Contains("-a"))
            {
                filename = filename.Replace("-a", "");
                append = true;
            }
            filename = filename.Trim();
            if (File.Exists(filename) && force.Equals(false))
            {
                ConsoleWrite($"teeCommand: Run(): output file \"{filename}\" exists. Use parameter -f to force");
                return;
            }
            using (StreamWriter file = new StreamWriter(filename, append))
            {
                file.Write(last_message);
            }            
        }
        public override string Help(params string[] help_args)
        {
            return "Send input to file. Use parameter -f to force, -a to append";
        }
    }
}