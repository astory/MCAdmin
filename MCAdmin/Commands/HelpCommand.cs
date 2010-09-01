using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class HelpCommand : Command
    {
        public HelpCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string str = "";
                int curlvl = ply.GetLevel();
                foreach (KeyValuePair<string, Command> kv in parent.commands)
                {
                    if (kv.Value.minlevel <= curlvl && kv.Key != "commands") str += "!" + kv.Key + ", ";
                }
                ply.SendDirectedMessage("Available commands: " + str.Remove(str.Length - 2));
                ply.SendDirectedMessage("For more help use !help command");
                ply.SendDirectedMessage("Do not type <> or [] around parameters.");
                ply.SendDirectedMessage("<> means the parameter is required, [] that it is optional");
            }
            else
            {
                string cmdStr = cmdparts[1].ToLower();
                if (cmdStr[0] == '!' || cmdStr[0] == '/') cmdStr = cmdStr.Substring(1);
                if (!parent.commands.ContainsKey(cmdStr)) { ply.SendDirectedMessage("Unknown command!"); return; }
                Command cmd = parent.commands[cmdStr];
                ply.SendDirectedMessage(cmd.Help);
                ply.SendDirectedMessage("Usage: !" + cmdStr + " " + cmd.Usage);
            }
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives help about commands."; } }
        public override string Usage { get { return "[command]"; } }
    }
}
