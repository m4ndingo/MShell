using System;
using System.Collections.Generic;

namespace testsc
{
    internal class CoreCommand //: Core
    {
        public enum RESULT_TYPE
        {
            NONE        = 0,
            TEXT        = 1,
            COMMANDS    = 2
        };
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
            if (Core.silent.Equals(false))
                Console.WriteLine(output);
            Core.last_message += Core.last_message.Length == 0 ? output : '\n' + output;
        }
    }
}