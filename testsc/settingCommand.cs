using System.Collections.Generic;
using System.Linq;

namespace testsc
{
    class settingCommand : CoreCommand
    {
        public settingCommand()
        {
            if (Core.aliases.Count.Equals(0))
                LoadSettings();
        }
        public override string Help(params string[] help_args)
        {
            string name = help_args[0];
            string help = "";
            if (name.Equals("set"))
                help = string.Format("I'm the settings command. Type \"{0}\" or \"{0} [setting] [value]\" to add a new setting", name);
            else if (name.Equals("unset"))
                help = string.Format("Type \"{0} [setting]\" to remove setting", name);
            else
            {
                if (Core.settings.ContainsKey(name))
                    help += string.Format("Try \"{0} [value]\" to change its value. Current value is \"{1}\"", name, Core.EncodeNoAscii(Core.settings[name]));
                else
                    help += string.Format("Try \"set {0} [value]\" to set its value", name);                
            }
            return help;
        }
        public override void Run()
        {
            if (this.cmd_without_args.Equals("set"))
            {
                if (this.args.Contains(" "))
                    AddSetting();
                else
                    DumpSettings();
            }
            else if (this.cmd_without_args.Equals("unset"))
            {
                RemoveSetting();
            }
            else
            {
                if (isValidSetting(this.cmd_without_args).Equals(false))
                {
                    ConsoleWrite_Atom("Run(): settingCommand : CoreCommand - Invalid Setting");
                    return;
                }
                UpdateSetting(this.cmd_without_args, this.args);
            }
            this.result_type = RESULT_TYPE.NONE;
        }

        private void AddSetting()
        {
            string[] args = this.args.Split(' ');
            string setting = args[0];
            string value = string.Join(" ", args.Skip(1));
            Core.settings[setting] = value;
            Core.core_commands[setting] = Core.settingsManager;

            // save to shared array (static objects share vars)
            Core.RegisterNewProperty(setting, new Core.CommandProperty { is_setting = true, input_type=CoreCommand.INPUT_TYPE.PARAMS });
        }
        private void RemoveSetting()
        {
            if(this.args.Length.Equals(0))
            {
                ConsoleWrite_Atom("Removesetting(): settingCommand : setting name is missing");
                return;
            }
            if (Core.settings.ContainsKey(this.args).Equals(false)) return;
            Core.settings.Remove(this.args);
            Core.core_commands.Remove(this.args);
        }
        private void DumpSettings()
        {
            if (args.Length > 0)
            {
                if (Core.settings.ContainsKey(args).Equals(false))
                {
                    ConsoleWrite_Atom("DumpSettings(): Setting \"{0}\" not found", args);
                    return;
                }
                ConsoleWrite_Atom("{0};{1}", args, Core.EncodeNoAscii(Core.settings[args]));
                return;
            }
            foreach (KeyValuePair<string, string> setting in Core.settings)
            {
                ConsoleWrite("set", "{0};{1}", setting.Key, Core.EncodeNoAscii(setting.Value));
            }
        }

        private bool isValidSetting(string key)
        {
            return Core.settings.ContainsKey(key);
        }
        public void LoadSettings()
        {
            Core.settings["loop"] = "1";
            Core.settings["PS1"] = "{ESC}[33m? {DEF}";
        }
        private void UpdateSetting(string name, string value)
        {
            if(value.Equals(""))
            {
                ConsoleWrite_Atom(Core.settings[name]);
                return;
            }
            Core.settings[name] = value;
        }
    }
}
