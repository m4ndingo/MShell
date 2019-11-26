namespace testsc
{
    class echoCommand : CoreCommand
    {
        public override void Run()
        {
            ConsoleWrite(args);
        }
        public override string Help(params string[] help_args)
        {
            return "Writes text";
        }
    }
}