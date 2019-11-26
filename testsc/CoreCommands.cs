﻿using System;

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
        // results
        public string results = null;
        public RESULT_TYPE result_type = RESULT_TYPE.TEXT;

        public virtual void Run()
        {
            ConsoleWrite("Run(): CoreCommand : Core");
        }
        public static void ConsoleWrite(string mesage, params object[] args)
        {
            Console.WriteLine(mesage, args);
        }
    }
}