using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace testsc
{
    class Prompt
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        public string PS1 = "? ";
        public bool loop = true;
        private Core core = null;

        public Prompt(string newPS1=null, Core core=null)
        {
            this.PS1 = string.IsNullOrEmpty(newPS1) ? PS1 : newPS1;
            this.core = core;
            enableAnsi();
            refreshVars();
        }
        public IEnumerable<string> doLoop()
        {
            while (this.loop)
            {                
                Console.Write(this.PS1);                
                yield return Console.ReadLine();
                refreshVars();
            }
            yield return null;
        }
        private void refreshVars()
        {
            this.loop = Core.readSetting("loop", "0").Equals("1");
            this.PS1 = Core.readSetting("PS1", this.PS1);
        }

        private void enableAnsi()
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
        }
    }
}