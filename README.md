# MShell
Tiny customizable dot net shell test (alpha) project :)

Every command has its own .cs file for easy cut&paste and usage in other projects

```bash
[ under construction ]

Current help:

$ help
?           PARAMS   Alias '?' current value 'help'
help        PARAMS   I'm the help. Try: "help" or "help [command]"
ls          PARAMS   List files in current directory. Flags: -l for extended info. Settings: curdir
cat         PARAMS   Dumps file contents. Usage: cat <filename>
grep        PIPE     Grep lines using regex. Settings: ignorecase escapeargs (pipe)
replace     PIPE     Replace lines using regular expressions. Settings: ignorecase (pipe)
alias       HYBRID   I'm the alias command. Try: alias or alias [name] or alias [newalias] [commands]
set         PARAMS   I'm the settings command. Type "set" or "set [setting] [value]" to add a new setting
unset       PARAMS   Try "set unset [value]" to set its value (setting)
loop        PARAMS   Try "loop [value]" to change its value. Current value is "1" (setting)
echo        PARAMS   Writes text
nl          PIPE     Enumerate lines (pipe)
np          PIPE     Send to namedpipe. Settings: npipe (pipe)
wc          PIPE     Count number of lines (pipe)
tee         PIPE     Send piped data to file. Flags: -f to force, -a to append (pipe)
exec        PIPE     Execute piped commands (pipe)
PS1         PARAMS   Try "PS1 [value]" to change its value. Current value is "$ " (setting)
ignorecase  PARAMS   Try "set ignorecase [value]" to set its value (setting)
last        NONE     Show last output
!           PARAMS   Executes external programs. Settings: consoleap. Ex: ! notepad
q           NONE     Alias 'q' current value 'loop 0'
```
