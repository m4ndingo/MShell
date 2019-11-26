namespace testsc
{
    class wcCommand : CoreCommand
    {
        public wcCommand()
        {
            this.isPipe = true;
        }
        public override void Run()
        {
            string filename = this.args;
            ConsoleWrite(last_message.Split('\n').Length.ToString());
        }
        public override string Help(params string[] help_args)
        {
            return "count lines";
        }
    }
}
