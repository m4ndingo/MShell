namespace testsc
{
    class wcCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Count number of lines";
        }
        public override void Run()
        {
            string filename = this.args;
            ConsoleWrite_Atom(last_message.Split('\n').Length.ToString());
        }
    }
}
