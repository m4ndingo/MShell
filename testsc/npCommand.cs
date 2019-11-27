using System;
using System.IO;
using System.IO.Pipes;

namespace testsc
{
    class npCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Send to namedpipe. Settings: npipe";
        }
        public override void Run()
        {
            ConsoleWrite(SendToNamedPipe(last_message, Core.readSetting("npipe", args)));
        }

        private string SendToNamedPipe(string message, string namedpipe_name)
        {
            if (message.Length == 0) return "";
            if (namedpipe_name.Equals(""))
                return "npCommand: SendToNamedPipe(): missing namedpipe arg or \"npipe\" setting";
            
            NamedPipeClientStream client = null;
            client = new NamedPipeClientStream(namedpipe_name);
            try
            {
                client.Connect(1000);
            }
            catch
            {
                return $"npCommand: SendToNamedPipe(): timeout connecting to \"{namedpipe_name}\"";
            }
            string result = "";
            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);
            writer.AutoFlush = true;
            try
            {
                writer.Write(message);
                result = reader.ReadToEnd();
                client.Close();
            }
            catch (Exception ex)
            {
                result = string.Format("npCommand: SendToNamedPipe(): {0}", ex.Message);
            }
            return result;
        }    
    }
}