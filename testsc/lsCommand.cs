using System;
using System.IO;

namespace testsc
{
    class lsCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "List files in current directory. Flags: -l for extended info";
        }
        public override void Run()
        {
            bool extended = args.Contains("-l");
            if (extended) args = args.Replace("-l", "");
            string path = args.Trim();
            string dirname;

            bool isFile = File.Exists(path);
            try
            {
                dirname = Path.GetDirectoryName(path + (isFile?"":"\\"));
            }catch(Exception ex)
            {
                ConsoleWrite_Atom("lsCommand: Run(): {0}", ex.Message.ToString());
                return;
            }
            string match = Path.GetFileName(path);
            if (match.Equals("")) match = "*.*";
            if (dirname == null) dirname = path;
            if (dirname.Equals("")) dirname = ".";
            string[] fileEntries, dirEntries;
            try
            {
                dirEntries = Directory.GetDirectories(dirname);
                fileEntries = Directory.GetFiles(dirname, match);
            }
            catch(Exception ex)
            {
                ConsoleWrite_Atom("lsCommand: Run(): {0}", ex.Message.ToString());
                return;
            }
            if (isFile.Equals(false))
            {
                foreach (string real_dirName in dirEntries)
                {
                    string dirName = real_dirName.Replace("/", @"\"); //win style
                    if (extended)
                    {
                        DirectoryInfo info = new DirectoryInfo(dirName);
                        ConsoleWrite("ls", "dir;" + (fileEntries.Length > 0 ? ";" : "") + "{0};{1}", info.CreationTime.ToString(), dirName);
                    }
                    else
                    {
                        ConsoleWrite("ls", dirName + @"\");
                    }
                }
            }
            foreach (string fileName in fileEntries)
            {
                string baseName = fileName;
                if (fileName.StartsWith(@".\"))
                    baseName = fileName.Substring(2);
                
                if (extended)
                {
                    FileInfo info = new FileInfo(baseName);
                    ConsoleWrite("ls", "file;{0};{1};{2}", info.Length.ToString(), info.CreationTime.ToString(), baseName);
                }
                else
                    ConsoleWrite("ls", baseName);
            }
        }
    }
}