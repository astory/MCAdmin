﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class HelpCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string str = "";
                int curlvl = ply.GetLevel();
                foreach (KeyValuePair<string, Command> kv in Program.commands)
                {
                    if (kv.Value.minlevel <= curlvl) str += "!" + kv.Key + ", ";
                }
                ply.SendDirectedMessage("Available commands: " + ((str.Length > 2) ? str.Remove(str.Length - 2) : "None"));
                ply.SendDirectedMessage("For more help use !help command");
                ply.SendDirectedMessage("Do not type <> or [] around parameters.");
                ply.SendDirectedMessage("<> means the parameter is required, [] that it is optional");
            }
            else
            {
                string cmdStr = cmdparts[1].ToLower();
                if (cmdStr[0] == '!' || cmdStr[0] == '/') cmdStr = cmdStr.Substring(1);
                if (!Program.commands.ContainsKey(cmdStr)) { ply.SendDirectedMessage("Unknown command!"); return; }
                Command cmd = Program.commands[cmdStr];
                ply.SendDirectedMessage(cmd.Help);
                ply.SendDirectedMessage("Usage: !" + cmdStr + " " + cmd.Usage);
            }
        }

        public override int minlevel { get { return reqlevel; } set { } }
        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives help about commands."; } }
        public override string Usage { get { return "[command]"; } }
    }
}
