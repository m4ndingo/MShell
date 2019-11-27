﻿namespace testsc
{
    class lastCommand : CoreCommand
    {
        public override void Run()
        {
            ConsoleWrite(Core.getLastMessage());
        }
        public override string Help(params string[] help_args)
        {
            return "Show last output";
        }
    }
}