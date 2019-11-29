using System.IO;

namespace testsc
{
    class echoCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Writes text";
        }
        public override void Run()
        {
            ConsoleWrite_Atom(args);
        }
    }
}