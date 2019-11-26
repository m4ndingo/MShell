using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testsc
{
    class nlCommand : CoreCommand
    {
        public override void Run()
        {
            int idx = 0;
            foreach (string line in last_message.Split('\n'))
                ConsoleWrite("{0};{1}", (idx++).ToString(), line);
        }
    }
}