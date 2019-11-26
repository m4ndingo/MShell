namespace testsc
{
    class execCommand : CoreCommand
    {
        public execCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {            
            foreach (string line in last_message.Split('\n'))
                Core.ParseTypedCmds(line);
        }
        public override string Help(params string[] help_args)
        {
            return "Execute lines";
        }
    }
}