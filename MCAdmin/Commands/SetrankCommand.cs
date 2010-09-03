using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class SetrankCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() <= Program.PlyGetLevel(cmdparts[1])) { ply.SendPermissionDenied(); return; }
            string rank = cmdparts[2].ToLower();
            if (!Program.ranklevels.ContainsKey(rank)) { ply.SendDirectedMessage("Rank does not exist!"); return; }
            if (Program.ranklevels[rank] >= ply.GetLevel()) { ply.SendPermissionDenied(); return; }
            Program.PlySetRank(cmdparts[1], rank);
            Program.SendServerMessage(ply.name + " set rank of " + cmdparts[1] + " to " + Program.PlyGetRank(cmdparts[1]));
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Sets rank of specified player to specified rank."; } }
        public override string Usage { get { return "<playername> <rankname>"; } }
    }
}
