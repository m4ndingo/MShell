namespace testsc
{
    class echoCommand : CoreCommand
    {
        public override void Run()
        {
            ConsoleWrite(args);
        }
    }
}