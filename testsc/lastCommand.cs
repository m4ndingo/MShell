namespace testsc
{
    class lastCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Show last output";
        }
        public override void Run()
        {
            ConsoleWrite_Atom(Core.getLastMessage());
        }
    }
}