using System.IO;
using System.Linq;

namespace testsc
{
    class catCommand : CoreCommand
    {
        public override void Run()
        {
            string filename = getParameter(args, "filename");
            if (File.Exists(filename))
                ConsoleWrite(string.Join("", File.ReadAllBytes(filename).Select(b => (char)b)));
            else
                ConsoleWrite($"catCommand: Run(): file \"{filename}\" not found or missing setting \"filename\"");
        }
        public override string Help(params string[] help_args)
        {
            return "cat <filename>";
        }
    }
}
