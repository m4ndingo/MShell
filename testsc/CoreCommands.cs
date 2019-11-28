using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace testsc
{
    internal class CoreCommand
    {
        public enum RESULT_TYPE
        {
            NONE        = 0,
            TEXT        = 1,
            COMMANDS    = 2
        };
        public enum INPUT_TYPE
        {
            NONE        = 0,
            PARAMS      = 1,
            PIPE        = 2,
            HYBRID      = 3
        }
        // for commands context
        public string cmd_with_args = null;
        internal string cmd_without_args;
        internal string args;
        internal bool isPipe;
        internal string last_message;

        // results
        public string results = null;
        public RESULT_TYPE result_type = RESULT_TYPE.TEXT;

        public virtual void Run()
        {
            ConsoleWrite("Run(): CoreCommand : Core");
        }
        public virtual string Help(params string[] help_args)
        {
            return string.Format("no help for '{0}'", help_args[0]);
        }
        public static void ConsoleWrite(string message, params string[] args)
        {
            string output = args.Length > 0 ? string.Format(message, args) : message;
            if (output.Length > 0 && Core.silent.Equals(false)) 
            {
                Console.WriteLine(AnsiColorize(output));
            }
            Core.last_message += Core.last_message.Length == 0 ? output : '\n' + output;
        }
        // return commandline arguments if found, else return setting or default value
        public static string getParameter(string args, string setting, string def = null)
        {
            return args.Length > 0 ? args : Core.readSetting(setting, def);
        }

        private static string AnsiColorize(string output)
        {
            char ESC = '\x1b';  // {ESC}[32m
            output = Regex.Replace(output,"([;<>\\|])", $"{ESC}[36m$1{ESC}[0m");
            output = Regex.Replace(output, "([│─┌┐└┘┬┴])", $"{ESC}[35m$1{ESC}[0m");
            return output;
        }
    }
}