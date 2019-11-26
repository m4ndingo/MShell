using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testsc
{
    class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();
            Prompt prompt = new Prompt("> ", core);
            foreach (string prompt_cmds in prompt.doLoop())
            {
                core.cmds(prompt_cmds);
            }
        }
    }
}
