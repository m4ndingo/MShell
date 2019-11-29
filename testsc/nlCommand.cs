namespace testsc
{
    class nlCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Enumerate lines";
        }
        public override void Run()
        {
            int idx = 0;
            foreach (string line in last_message.Split('\n'))
                ConsoleWrite("nl", "{0};{1}", (idx++).ToString(), line);
        }
    }
}