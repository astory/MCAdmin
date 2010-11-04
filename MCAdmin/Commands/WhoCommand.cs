﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class WhoCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string str = "";
                foreach (Player plyn in Program.minecraftFirewall.players)
                {
                    if (plyn.name == null || plyn.name == "") continue;
                    str += plyn.name + ", ";
                }
                ply.SendDirectedMessage("Connected players: " + ((str.Length > 2) ? str.Remove(str.Length - 2) : "None"));
            }
            else
            {
                Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
                if (ply2 == null) { ply.SendDirectedMessage("Sorry, player could not be found!"); return; }
                ply.SendDirectedMessage("Name: " + ply2.name);
                ply.SendDirectedMessage("Rank: " + ply2.GetRank());
                string plyn = ply2.name.ToLower();
                if (ply.GetLevel() >= 3)
                {
                    ply.SendDirectedMessage("IP: " + ply2.ip);
                }
            }
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Lists all players or gives info about a specific player."; } }
        public override string Usage { get { return "[playername]"; } }
    }
}
