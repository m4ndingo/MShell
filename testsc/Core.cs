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
                    if (cmd.result_type.Equals(CoreCommand.RESULT_TYPE.COMMANDS))
                    {
                        ParseTypedCmds((cmd.results + " " + cmd.args).TrimEnd());    // output are more commands 
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

            last_message            = "";           // clear last_write/last_message used between pipes

            // execute command
            cmd.Run();

            last_message_saved = last_message;

            return cmd;
        }

        static public bool isValidCommand(string cmd)
        {
            if (core_commands == null)
                LoadCommands();
            return core_commands != null && core_commands.ContainsKey(cmd);
        }

        
        static private void AddNewCommand(string command, CoreCommand helpManager, string props=null)
        {
            core_commands.Add(command, helpManager);
        }
        public static Dictionary<string, CommandProperty> command_properties = new Dictionary<string, Core.CommandProperty>();
        static private void AddNewCommand(string command, CoreCommand helpManager, CommandProperty properties)
        {
            core_commands.Add(command, helpManager);
            RegisterNewProperty(command, properties);
        }
        static public void RegisterNewProperty(string command, CommandProperty properties)
        { 
            command_properties[command] = properties;
        }                    

        static public CommandProperty getCommandProperties(string cmd)
        {
            if (command_properties.ContainsKey(cmd).Equals(false))
                return null;
            return command_properties[cmd];
        }

        // properties and configuration for managed command objects
        public class CommandProperty
        {
            public CoreCommand.INPUT_TYPE input_type { get; set; }
        }

        static private void LoadValidCommands()
        {
            core_commands = new Dictionary<string, CoreCommand>();

            // avalable configurations
            CommandProperty isPipe = new CommandProperty() { input_type = CoreCommand.INPUT_TYPE.PIPE };
            CommandProperty isHybrid = new CommandProperty() { input_type = CoreCommand.INPUT_TYPE.HYBRID };
            CommandProperty noParams = new CommandProperty() { input_type = CoreCommand.INPUT_TYPE.NONE };
            CommandProperty Params = new CommandProperty() { input_type = CoreCommand.INPUT_TYPE.PARAMS };

            AddNewCommand("?", helpManager, Params);
            AddNewCommand("help", helpManager, Params);
            AddNewCommand("ls", new lsCommand(), Params);
            AddNewCommand("cat", new catCommand(), Params);
            AddNewCommand("grep", new grepCommand(), isPipe);
            AddNewCommand("replace", new replaceCommand(), isPipe);
            AddNewCommand("alias", aliasManager, isHybrid);
            AddNewCommand("set", settingsManager, Params);
            AddNewCommand("unset", settingsManager, Params);
            AddNewCommand("loop", settingsManager, Params);
            AddNewCommand("echo", new echoCommand(), Params);
            AddNewCommand("nl", new nlCommand(), isPipe);
            AddNewCommand("np", new npCommand(), isPipe);
            AddNewCommand("wc", new wcCommand(), isPipe);
            AddNewCommand("tee", new teeCommand(), isPipe);
            AddNewCommand("exec", new execCommand(), isPipe);
            AddNewCommand("PS1", settingsManager, Params);
            AddNewCommand("ignorecase", settingsManager, Params);
            AddNewCommand("last", new lastCommand(), noParams);
            AddNewCommand("!", new systemCommand(), Params);
        }
        static private void LoadCommands()
        {

            LoadValidCommands();
            Load_Alias();
        }
        private static void Load_Alias()
        {
            aliasManager.AddAlias("q", "loop 0", CoreCommand.INPUT_TYPE.NONE);
            aliasManager.AddAlias("?", "help");
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