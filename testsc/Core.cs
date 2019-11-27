using System.Linq;
using System.Collections.Generic;
using System;

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
        //
        static private string last_message_saved = "";
        static public string last_message = "";
        static public bool silent = false;
        public void cmds(string prompt_cmds)
        {            
            ParseTypedCmds(prompt_cmds);
        }

        static public void ParseTypedCmds(string prompt_cmds)
        {
            if (string.IsNullOrEmpty(prompt_cmds))
                return;
            prompt_cmds = EscapeSpecialChars(prompt_cmds);
            foreach (string cmd_with_args_and_pipes in prompt_cmds.Split(';'))          // cmd1;cmd2|cmd3
            {
                bool isPipe = false;
                string[] cmd_with_args_pipeables = cmd_with_args_and_pipes.Split('|');
                silent = cmd_with_args_pipeables.Length > 1;
                for (int i = 0; i < cmd_with_args_pipeables.Length; i++) 
                {
                    string cmd_with_args = UnescapeSpecialChars(cmd_with_args_pipeables[i]);
                    if (i == cmd_with_args_pipeables.Length - 1) silent = false;
                    CoreCommand cmd = ParseCmd(cmd_with_args, isPipe);
                    if (cmd == null)
                        continue;
                    if (cmd.result_type.Equals(CoreCommand.RESULT_TYPE.COMMANDS)/* || cmd.result_type.Equals(CoreCommand.RESULT_TYPE.NONE)*/)
                    {
                        ParseTypedCmds(cmd.results);
                    }
                    else if (cmd.results != null && cmd.result_type.Equals(CoreCommand.RESULT_TYPE.NONE).Equals(false)) 
                    {
                        CoreCommand.ConsoleWrite("Results: '{0}' Type: {1}", cmd.results, cmd.result_type.ToString());
                    }
                    isPipe = true;
                }
            }
            Core.last_message = "";
        }
        static public string EscapeSpecialChars(string commands)
        {
            commands = commands.Replace("\\|", "{PIPE}");
            commands = commands.Replace("\\;", "{SC}");
            commands = commands.Replace("\\<", "{LT}");
            commands = commands.Replace("\\>", "{GT}");
            return commands;
        }
        static public string UnescapeSpecialChars(string commands)
        {
            commands = commands.Replace("{PIPE}", "|");
            commands = commands.Replace("{SC}", ";");
            commands = commands.Replace("{LT}", "<");
            commands = commands.Replace("{GT}", ">");
            // colors
            commands = commands.Replace("{GREEN}", "{ESC}[32m");
            commands = commands.Replace("{DEF}", "{ESC}[0m");
            // esc
            commands = commands.Replace("{ESC}", "\x1b");
            return commands;
        }
        static public string readSetting(string name, string def = null)
        {
            return settings.ContainsKey(name) ? settings[name] : def;
        }

        static private CoreCommand ParseCmd(string cmd_with_args, bool isPipe = false)
        {
            KeyValuePair<string, string> kCmd = getCmdArgs(cmd_with_args);
            if (isValidCommand(kCmd.Key).Equals(false))
            {
                CoreCommand.ConsoleWrite("command '{0}' not found", kCmd.Key);
                last_message_saved = last_message;
                return null;
            }
            // load command
            CoreCommand cmd = Core.core_commands[kCmd.Key];
            // prepare context
            cmd.cmd_with_args       = cmd_with_args;
            cmd.cmd_without_args    = kCmd.Key;
            cmd.args                = kCmd.Value;
            cmd.isPipe              = isPipe;
            cmd.last_message        = last_message;

            last_message            = "";      // clear last_write/last_message used between pipes

            // execute command
            cmd.Run();

            last_message_saved = last_message;

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
            core_commands.Add("help", helpManager);
            core_commands.Add("ls", new lsCommand());
            core_commands.Add("cat", new catCommand());
            core_commands.Add("grep", new grepCommand());
            core_commands.Add("replace", new replaceCommand());
            core_commands.Add("alias", aliasManager);
            core_commands.Add("set", settingsManager);
            core_commands.Add("unset", settingsManager);
            core_commands.Add("loop", settingsManager);
            core_commands.Add("echo", new echoCommand());
            core_commands.Add("nl", new nlCommand());
            core_commands.Add("np", new npCommand());
            core_commands.Add("wc", new wcCommand());
            core_commands.Add("tee", new teeCommand());
            core_commands.Add("exec", new execCommand());
            core_commands.Add("PS1", settingsManager);
            core_commands.Add("ignorecase", settingsManager);
            core_commands.Add("last", new lastCommand());
            core_commands.Add("!", new systemCommand());
            core_commands.Add("?", helpManager);
            core_commands.Add("q", aliasManager);
        }

        static public KeyValuePair<string, string> getCmdArgs(string cmd_with_args)
        {
            string[] my_args = cmd_with_args.Split(' ');
            return new KeyValuePair<string, string>(my_args[0], string.Join(" ", my_args.Skip(1)));
        }
        static public string getLastMessage()
        {
            return last_message_saved;
        }
    }
}