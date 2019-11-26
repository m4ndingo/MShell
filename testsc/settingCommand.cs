using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testsc
{
    class settingCommand : CoreCommand
    {
        public settingCommand()
        {
            if (Core.aliases.Count.Equals(0))
                LoadSettings();
        }
        public override void Run()
        {
            //ConsoleWrite("Run(): settingCommand : CoreCommand - Running: {0}", this.cmd_with_args);            
            if (this.cmd_without_args.Equals("set"))
            {
                if (this.args.Contains(" "))
                    AddSetting();
                else
                    DumpSettings();
            }
            else
            {
                if (isValidSetting(this.cmd_without_args).Equals(false))
                {
                    ConsoleWrite("Run(): settingCommand : CoreCommand - Invalid Setting");
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
        }
        private void DumpSettings()
        {
            if (args.Length > 0)
            {
                if (Core.settings.ContainsKey(args).Equals(false))
                {
                    ConsoleWrite("DumpSettings(): Setting \"{0}\" not found", args);
                    return;
                }
                ConsoleWrite("{0};{1}", args, Core.settings[args]);
                return;
            }
            foreach (KeyValuePair<string, string> setting in Core.settings)
            {
                ConsoleWrite("{0};{1}", setting.Key, setting.Value);
            }
        }

        private bool isValidSetting(string key)
        {
            return Core.settings.ContainsKey(key);
        }
        public void LoadSettings()
        {
            Core.settings.Clear();
            Core.settings.Add("loop", "1");
            Core.settings.Add("PS1", "$ ");
        }
        private void UpdateSetting(string name, string value)
        {
            if(value.Equals(""))
            {
                ConsoleWrite(Core.settings[name]);
                return;
            }
            Core.settings[name] = value;
        }
        public override string Help(params string[] help_args)
        {
            string name = help_args[0];
            string help= string.Format("I'm the settings command. Type \"{0}\" for reading or \"{0} [setting] [value]\" to add a new setting", name); 
            if (Core.settings.ContainsKey(name))
            {
                help = string.Format("Setting \"{0}\". Try \"{0} [value]\" to change its value", name);
                help += string.Format(". Current value is \"{0}\"", Core.settings[name]);
            }
            return help;
        }
    }
}
