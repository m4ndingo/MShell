using System.IO;
using System.Linq;

namespace testsc
{
    class catCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Dumps file contents. Usage: cat <filename>";
        }
        public override void Run()
        {
            string filename = getParameter(args, "filename");
            if (File.Exists(filename))
            {
                byte[] bytes = File.ReadAllBytes(filename);
                if (bytes.Length > 0)
                    ConsoleWrite(string.Join("", bytes.Select(b => (char)b)));
            }
            else
                ConsoleWrite($"catCommand: Run(): file \"{filename}\" not found or missing setting \"filename\"");
        }
    }
}
