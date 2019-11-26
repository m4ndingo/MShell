namespace testsc
{
    class nlCommand : CoreCommand
    {
        public nlCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {
            int idx = 0;
            foreach (string line in last_message.Split('\n'))
                ConsoleWrite("{0};{1}", (idx++).ToString(), line);
        }
        public override string Help(params string[] help_args)
        {
            return "Enumerate lines";
        }
    }
}