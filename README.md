# MShell
Tiny customizable dot net shell test (alpha) project :)

Every command has its own .cs file for easy cut&paste and usage in other projects

```bash
[ under construction ]

Current help:

? help
?           PARAMS  A $ help
help        PARAMS    I'm the help. Try: "help" or "help [command]"
ls          PARAMS    List files in current directory. Flags: -l for extended info
cat         PARAMS    Dumps file contents. Usage: cat <filename>
grep        PIPE      Grep lines using regex. Settings: ignorecase escapeargs
match       PIPE      Match data using regular expresions
replace     PIPE      Replace lines using regular expressions. Settings: ignorecase
alias       HYBRID    I'm the alias command. Try: alias or alias [name] or alias [newalias] [commands]
set         PARAMS    I'm the settings command. Type "set" or "set [setting] [value]" to add a new setting
unset       PARAMS S  Type "unset [setting]" to remove setting
var         HYBRID    Pipe data to @variable or read with "var @variable"
loop        PARAMS S  Try "loop [value]" to change its value. Current value is "1"
echo        PARAMS    Writes text
nl          PIPE      Enumerate lines
np          PIPE      Send to namedpipe. Settings: npipe
wc          PIPE      Count number of lines
tee         PIPE      Send piped data to file. Flags: -f to force, -a to append
uniq        PIPE      Get unique lines
exec        PIPE      Execute piped commands
decode      PIPE      Decode input. Available decoders: b64 zlib gzip
table       PIPE      Build table from input
PS1         PARAMS S  Try "PS1 [value]" to change its value. Current value is "{ESC}[33m? {DEF}"
ignorecase  PARAMS S  Try "set ignorecase [value]" to set its value
last        NONE      Show last output
!           PARAMS    Executes external programs. Settings: consoleap. Ex: ! notepad
dbz         PIPE    A $ decode b64|decode zlib                       // Decodes base64 then zlib
lls         PARAMS  A $ ls -l {ARGS} |table                          // List files as table
strings     PIPE    A $ replace \x00|match [\x20-\x7f]{4,}|uniq      // Extract strings from files
alog        PARAMS  A $ cat {ARGS}|match (^.+?)\s-.+?\[(.+?)\].+?"(.+?)" {0,14}\;{2}|table|uniq  // Apache log as table
q           NONE    A $ loop 0                                       // Close shell
```
### Aliases
```
? alias|table
┌───────┬─────────────────────────────────────────────────────────────────────────┐
│dbz    │decode b64|decode zlib                                                   │
│lls    │ls -l {ARGS} |table                                                      │
│strings│replace \x00|match [\x20-\x7f]{4,}|uniq                                  │
│alog   │cat {ARGS}|match (^.+?)\s-.+?\[(.+?)\].+?"(.+?)" {0,14};{2}|table|uniq   │
│q      │loop 0                                                                   │
│?      │help                                                                     │
└───────┴─────────────────────────────────────────────────────────────────────────┘

? lls
┌────┬─────┬───────────────────┬─────────────────┐
│file│191  │27/11/2019 17:57:29│alias            │
│file│56684│26/11/2019 18:22:50│foo              │
│file│28   │28/11/2019 18:47:00│settings         │
│file│32256│26/11/2019 18:10:15│testsc.exe       │
│file│189  │26/11/2019 18:10:15│testsc.exe.config│
│file│91648│26/11/2019 18:10:15│testsc.pdb       │
└────┴─────┴───────────────────┴─────────────────┘

? cat testsc.exe|strings
!This program cannot be run in DOS mode.
$PEL
 H.text
 `.rsrc
v@@.reloc
+"r8
, r<
,+r2
, rr
+|r(
*.sw
+>+o
...

? alias alog
alog;cat {ARGS}|match (^.+?)\s-.+?\[(.+?)\].+?"(.+?)" {0,14}\;{2}|table|uniq
? alog foo\|grep wp-admin/
┌──────────────┬────────────────────────────────────────────────────────┐
│ 85.107.17.121│GET /wp-admin/ HTTP/1.1                                 │
│ 85.220.201.48│GET /wp-admin/ HTTP/1.1                                 │
│   9.70.200.38│GET /web/wp-admin/css/login.min.css?ver=5.2.3 HTTP/1.1  │
│   9.70.200.38│GET /web/wp-admin/css/forms.min.css?ver=5.2.3 HTTP/1.1  │
│   9.70.200.38│GET /web/wp-admin/css/l10n.min.css?ver=5.2.3 HTTP/1.1   │
│ 85.220.201.38│GET /web/wp-admin/css/login.min.css?ver=5.2.3 HTTP/1.1  │
│ 85.220.201.58│GET /web/wp-admin/css/forms.min.css?ver=5.2.3 HTTP/1.1  │
│ 85.220.201.58│GET /web/wp-admin/css/l10n.min.css?ver=5.2.3 HTTP/1.1   │
│  09.70.200.18│GET /web/wp-admin/css/install.min.css?ver=5.2.3 HTTP/1.1│
└──────────────┴────────────────────────────────────────────────────────┘
```
### Using pipes

#### Examples:
```
? cat foo|wc
269
? cat foo|grep wp-admin/|wc
31
# extract ips from apache log
? cat foo|match (^.+?\s)- {0}|uniq
85.17.47.11
127.0.0.1
8.78.253.14
85.220.11.48
6.158.9.191
19.70.10.28
::1
12.8.7.1
14.22.43.1
15.22.20.11
```
