using System.Linq;
using System.Collections.Generic;
using System;

namespace testsc
{
    class Core
    {
        // commands
        static public Dictionary<string, CoreCommand> core_commands = null;        
        // properties
        public static Dictionary<string, CommandProperty> command_properties = new Dictionary<string, Core.CommandProperty>();

        // dictionaries
        static public Dictionary<string, string> settings = new Dictionary<string, string>();
        static public Dictionary<string, string> aliases = new Dictionary<string, string>();
        static public Dictionary<string, string> variables = new Dictionary<string, string>();

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

        static public string ParseTypedCmds(string prompt_cmds, bool force_silent = false)
        {
            if (string.IsNullOrEmpty(prompt_cmds))
                return "";
            
            prompt_cmds = TagCommandlineChars(prompt_cmds);
            string lres = "", last = "";
            CoreCommand cmd = null;
            foreach (string cmd_with_args_and_pipes in prompt_cmds.Split(';'))          // cmd1;cmd2|cmd3
            {
                //bool isPipe = false;
                string[] cmd_with_args_pipeables = cmd_with_args_and_pipes.Split('|');
                silent = cmd_with_args_pipeables.Length > 1;
                for (int i = 0; i < cmd_with_args_pipeables.Length; i++) 
                {
                    bool isLast = i == cmd_with_args_pipeables.Length - 1;
                    string cmd_with_args = UntagCommandlineChars(cmd_with_args_pipeables[i]);                    
                    if (!force_silent  && isLast) silent = false;
                    cmd = ParseCmd(cmd_with_args, last_message);
                    //Console.WriteLine("debug>> cmd '{0}', piping '{1}' bytes, result '{2}' bytes type '{3}",
                    //    cmd_with_args, last_message.Length, cmd.results.Length, cmd.result_type);
                    if (cmd == null)
                        continue;
                    if (cmd.result_type.Equals(CoreCommand.RESULT_TYPE.COMMANDS)/* || cmd.result_type.Equals(CoreCommand.RESULT_TYPE.NONE)*/)
                    {
                        string new_command = cmd.results;
                        if (new_command.Contains("{ARGS}"))
                            new_command = new_command.Replace("{ARGS}", cmd.args);
                        else
                            new_command += " " + cmd.args;                        
                        lres = ParseTypedCmds(new_command, !isLast && Core.getCommandProperties(cmd_with_args).is_alias);    // output are more commands 
                        last_message = lres;
                    }
                    else if (cmd.results != null && cmd.result_type.Equals(CoreCommand.RESULT_TYPE.NONE).Equals(false)) 
                    {
                        //CoreCommand.ConsoleWrite(last_message);
                        //CoreCommand.ConsoleWrite("Results: '{0}' Type: {1}", cmd.results, cmd.result_type.ToString());
                    }                    
                }
                last = Core.last_message;
                Core.last_message = "";
            }
            return last;
        }
        // properties and configuration for managed command objects
        public class CommandProperty : ICloneable
        {
            public CoreCommand.INPUT_TYPE input_type { get; set; }
            public string help { get; set; }
            public bool is_setting { get; set; }
            public bool is_alias { get; set; }
            public object Clone()
            {
                return this.MemberwiseClone();
            }
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
            AddNewCommand("match", new matchCommand(), isPipe);
            AddNewCommand("replace", new replaceCommand(), isPipe);
            AddNewCommand("alias", aliasManager, isHybrid);
            AddNewCommand("set", settingsManager, Params);
            AddNewCommand("unset", settingsManager, Params);
            AddNewCommand("var", new varCommand(), isHybrid);
            AddNewCommand("loop", settingsManager, Params);
            AddNewCommand("echo", new echoCommand(), Params);
            AddNewCommand("nl", new nlCommand(), isPipe);
            AddNewCommand("np", new npCommand(), isPipe);
            AddNewCommand("wc", new wcCommand(), isPipe);
            AddNewCommand("tee", new teeCommand(), isPipe);
            AddNewCommand("uniq", new uniqCommand(), isPipe);
            AddNewCommand("exec", new execCommand(), isPipe);
            AddNewCommand("decode", new decodeCommand(), isPipe);
            AddNewCommand("table", new tableCommand(), isPipe);
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
            CoreCommand.INPUT_TYPE Params = CoreCommand.INPUT_TYPE.PARAMS;
            aliasManager.AddAlias(  "dbz",
                                    "decode b64|decode zlib", 
                                    CoreCommand.INPUT_TYPE.PIPE, "Decodes base64 then zlib");
            aliasManager.AddAlias(  "lls",        
                                    "ls -l {ARGS} |table", 
                                    Params, "List files as table");
            aliasManager.AddAlias(  "strings",    
                                    "replace \x00|match [\\x20-\\x7f]{4,}|uniq", 
                                    CoreCommand.INPUT_TYPE.PIPE, "Extract strings from files");
            aliasManager.AddAlias(  "alog",       
                                    "cat {ARGS}|match (^.+?)\\s-.+?\\[(.+?)\\].+?\"(.+?)\" {0,14}\\;{2}|table|uniq", 
                                    Params, "Apache log as table");
            aliasManager.AddAlias(  "q", 
                                    "loop 0", 
                                    CoreCommand.INPUT_TYPE.NONE, "Close shell");
            aliasManager.AddAlias(  "?", 
                                    "help");
        }

        public static string UnescapeArgs(string text)
        {
            text = text.Replace("\\n", "\n");
            text = text.Replace("\\s", " ");
            text = text.Replace("\\;", ";");
            return text;
        }

        public static string EncodeNoAscii(string value)
        {
            value = value.Replace("\x00", "\\x00");
            value = value.Replace("\x1b", "\\x1b");
            return value;
        }

        static public string TagCommandlineChars(string commands)
        {
            commands = commands.Replace("\\|", "{PIPE}");
            commands = commands.Replace("\\;", "{SC}");
            commands = commands.Replace("\\<", "{LT}");
            commands = commands.Replace("\\>", "{GT}");
            return commands;
        }
        static public string UntagCommandlineChars(string commands)
        {
            commands = commands.Replace("{PIPE}", "|");
            commands = commands.Replace("{SC}", "\\;");
            commands = commands.Replace("{LT}", "<");
            commands = commands.Replace("{GT}", ">");
            // colors
            commands = commands.Replace("{GREEN}", "{ESC}[32m");
            commands = commands.Replace("{PINK}",  "{ESC}[35m");
            commands = commands.Replace("{DEF}", "{ESC}[0m");
            // esc
            commands = commands.Replace("{ESC}", "\x1b");
            return commands;
        }
        static public string readSetting(string name, string def = null)
        {
            return settings.ContainsKey(name) ? settings[name] : def;
        }

        static private CoreCommand ParseCmd(string cmd_with_args, string pipe_string = null)
        {
            KeyValuePair<string, string> kCmd = getCmdArgs(cmd_with_args);
            if (kCmd.Key.Length.Equals(0)) return null;
            if (isValidCommand(kCmd.Key).Equals(false))
            {
                CoreCommand.ConsoleWrite_Atom("command '{0}' not found", kCmd.Key);
                last_message_saved = last_message;
                return null;
            }
            // load command
            CoreCommand cmd = Core.core_commands[kCmd.Key];
            // prepare context
            cmd.results             = pipe_string;
            //cmd.result_type         = CoreCommand.RESULT_TYPE.NONE;
            cmd.cmd_with_args       = cmd_with_args;
            cmd.cmd_without_args    = kCmd.Key;
            cmd.args                = kCmd.Value;            
            cmd.last_message        = last_message;

            //last_message = "";           // clear last_write/last_message used between pipes

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
        
        static private void AddNewCommand(string command, CoreCommand commandManager, CommandProperty properties)
        {
            bool isSetting = command.Equals("set").Equals(false) && commandManager.Equals(settingsManager);
            bool isAlias   = commandManager.Equals(aliasManager);
            CommandProperty prop = (CommandProperty)properties.Clone();
            prop.is_setting = isSetting;
            core_commands.Add(command, commandManager);
            RegisterNewProperty(command, prop);
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