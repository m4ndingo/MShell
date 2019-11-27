namespace testsc
{
    class execCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Execute piped commands";
        }
        public override void Run()
        {            
            foreach (string line in last_message.Split('\n'))
                Core.ParseTypedCmds(line);
        }
    }
}