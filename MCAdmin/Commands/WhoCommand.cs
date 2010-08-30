using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class WhoCommand : Command
    {
        public WhoCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string str = "";
                foreach (Player plyn in parent.minecraftFirewall.players)
                {
                    if (plyn.name == null || plyn.name == "") continue;
                    str += plyn.name + ", ";
                }
                parent.SendDirectedMessage(ply, "Connected players: " + str.Remove(str.Length - 2));
            }
            else
            {
                Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
                if (ply2 == null) { parent.SendDirectedMessage(ply, "Sorry, player could not be found!"); return; }
                parent.SendDirectedMessage(ply, "Name: " + ply2.name);
                parent.SendDirectedMessage(ply, "Rank: " + parent.PlyGetRank(ply2.name));
                if (ply.GetLevel() >= 3)
                {
                    parent.SendDirectedMessage(ply, "IP: " + ply2.ip);
                }
            }
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Lists all players or gives info about a specific player."; } }
        public override string Usage { get { return "[playername]"; } }
    }
}
