using System;
using System.Linq;
using System.Collections.Generic;

namespace testsc
{
    class Core
    {
        // commands
        static public Dictionary<string, CoreCommand> core_commands = null;
        // dictionaries
        static public Dictionary<string, string> settings = new Dictionary<string, string>();
        static public Dictionary<string, string> aliases = new Dictionary<string, string>();
        // preloaded commands
        static public settingCommand settingsManager = new settingCommand();
        static public aliasCommand aliasManager = new aliasCommand();
        static public helpCommand helpManager = new helpCommand();
        public void cmds(string prompt_cmds)
        {
            
            ParseTypedCmds(prompt_cmds);
        }

        private void ParseTypedCmds(string prompt_cmds)
        {
            if (prompt_cmds == null)
                return;
            foreach (string cmd_with_args in prompt_cmds.Split(';')) 
            {
                CoreCommand cmd = ParseCmd(cmd_with_args);
                if (cmd == null)
                    continue;
                if (cmd.result_type.Equals(CoreCommand.RESULT_TYPE.COMMANDS) || cmd.result_type.Equals(CoreCommand.RESULT_TYPE.NONE))
                {
                    ParseTypedCmds(cmd.results);
                }
                else if (cmd.results != null) 
                {
                    CoreCommand.ConsoleWrite("Results: '{0}' Type: {1}", cmd.results, cmd.result_type.ToString());
                }
            }
        }

        public string readSetting(string name, string def = null)
        {
            return settings.ContainsKey(name) ? settings[name] : def;
        }

        private CoreCommand ParseCmd(string cmd_with_args)
        {
            KeyValuePair<string, string> kCmd = getCmdArgs(cmd_with_args);
            if (isValidCommand(kCmd.Key).Equals(false))
            {
                CoreCommand.ConsoleWrite("command '{0}' not found", kCmd.Key);
                return null;
            }
            // load command
            CoreCommand cmd = Core.core_commands[kCmd.Key];
            // prepare context
            cmd.cmd_with_args       = cmd_with_args;
            cmd.cmd_without_args    = kCmd.Key;
            cmd.args                = kCmd.Value;
            // execute command
            cmd.Run();

            return cmd;
        }

        static public bool isValidCommand(string cmd)
        {
            if (core_commands == null)
                LoadValidCommds();
            return core_commands != null && core_commands.ContainsKey(cmd);
        }

        static private void LoadValidCommds()
        {
            core_commands = new Dictionary<string, CoreCommand>();
            core_commands.Add("q", aliasManager);
            core_commands.Add("alias", aliasManager);
            core_commands.Add("loop", settingsManager);
            core_commands.Add("echo", new echoCommand());
            core_commands.Add("help", helpManager);
            core_commands.Add("?", helpManager);
        }

        public KeyValuePair<string, string> getCmdArgs(string cmd_with_args)
        {
            string[] my_args = cmd_with_args.Split(' ');
            return new KeyValuePair<string, string>(my_args[0], string.Join(" ", my_args.Skip(1)));
        }
    }
}