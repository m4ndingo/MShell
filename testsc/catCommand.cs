using System.IO;
using System.Linq;

namespace testsc
{
    class catCommand : CoreCommand
    {
        public override void Run()
        {
            string filename = this.args;
            if (File.Exists(filename))
                ConsoleWrite(string.Join("", File.ReadAllBytes(filename).Select(b => (char)b)));
        }
        public override string Help(params string[] help_args)
        {
            return "cat <filename>";
        }
    }
}
