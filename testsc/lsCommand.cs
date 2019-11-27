using System.IO;

namespace testsc
{
    class lsCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "List files in current directory. Flags: -l for extended info. Settings: curdir";
        }
        public override void Run()
        {
            bool extended = args.Contains("-l");
            string[] fileEntries = Directory.GetFiles(Core.readSetting("curdir", "."));
            foreach (string fileName in fileEntries)
            {
                string baseName = fileName;
                if (fileName.StartsWith(@".\"))
                    baseName = fileName.Substring(2);
                if (extended)
                {
                    FileInfo info = new FileInfo(baseName);
                    ConsoleWrite("{0};{1};{2}", info.CreationTime.ToString(),baseName, info.Length.ToString());
                }
                else
                    ConsoleWrite(baseName);
            }
        }
    }
}