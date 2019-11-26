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
            LoadSettings();
        }
        public override void Run()
        {
            //ConsoleWrite("Run(): settingCommand : CoreCommand - Running: {0}", this.cmd_with_args);
            LoadSettings();
            if (isValidSetting(this.cmd_without_args).Equals(false))
            {
                ConsoleWrite("Run(): settingCommand : CoreCommand - Invalid Setting");
                return;
            }

            UpdateSetting(this.cmd_without_args, this.args);
            this.result_type = RESULT_TYPE.NONE;
        }
        private bool isValidSetting(string key)
        {
            return Core.settings.ContainsKey(key);
        }
        public void LoadSettings()
        {
            Core.settings.Clear();
            Core.settings.Add("loop", "1");
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
    }
}
