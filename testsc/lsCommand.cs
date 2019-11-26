using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testsc
{
    class lsCommand : CoreCommand
    {
        public override void Run()
        {
            string[] fileEntries = Directory.GetFiles(Core.readSetting("curdir", "."));
            foreach (string fileName in fileEntries)
            {
                string baseName = fileName;
                if (fileName.StartsWith(@".\"))
                    baseName = fileName.Substring(2);
                ConsoleWrite(baseName);
            }
        }
        public override string Help(params string[] help_args)
        {
            return "List files in current directory";
        }
    }
}