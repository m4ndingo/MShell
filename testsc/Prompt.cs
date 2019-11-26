using System;
using System.Collections.Generic;

namespace testsc
{
    class Prompt
    {
        public string PS1 = "? ";
        public bool loop = true;
        private Core core = null;
        public Prompt(string newPS1=null, Core core=null)
        {
            this.PS1 = string.IsNullOrEmpty(newPS1) ? PS1 : newPS1;
            this.core = core;
        }
        public IEnumerable<string> doLoop()
        {
            while(this.loop)
            {
                Console.Write(this.PS1);
                yield return Console.ReadLine();
                this.loop = core.readSetting("loop", "0").Equals("1");
            }
            yield return null;
        }
    }
}