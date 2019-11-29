using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace testsc
{
    class systemCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Executes external programs. Settings: consoleap. Ex: ! notepad";
        }
        public override void Run()
        {
            KeyValuePair<string, string> kArgs = Core.getCmdArgs(args);
            bool consoleApp = Core.readSetting("consoleapp", "1").Equals("1");
            
            ProcessWindowStyle windowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            if (consoleApp == false)
                windowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = kArgs.Key;
            pProcess.StartInfo.Arguments = kArgs.Value;
            pProcess.StartInfo.UseShellExecute = !consoleApp;
            pProcess.StartInfo.RedirectStandardOutput = consoleApp;
            pProcess.StartInfo.WindowStyle = windowStyle;            
            try
            {
                if (consoleApp)
                {
                    pProcess.Start();
                    ConsoleWrite_Atom(pProcess.StandardOutput.ReadToEnd().TrimEnd());    // Output results
                    pProcess.WaitForExit();
                }
                else
                {
                    Task.Factory.StartNew(() =>
                    {
                        pProcess.Start();                     
                        // pProcess.WaitForExit();
                    });
                }
            }
            catch (Exception ex)
            {
                ConsoleWrite_Atom("systemCommand: {0}", ex.Message);
            }
        }
    }
}
