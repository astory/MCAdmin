using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class SetrankCommand : Command
    {
        public SetrankCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() <= parent.PlyGetLevel(cmdparts[1])) { parent.SendPermissionDenied(ply); return; }
            string rank = cmdparts[2].ToLower();
            if (!parent.ranklevels.ContainsKey(rank)) { parent.SendDirectedMessage(ply, "Rank does not exist!"); return; }
            if (parent.ranklevels[rank] >= ply.GetLevel()) { parent.SendPermissionDenied(ply); return; }
            parent.PlySetRank(cmdparts[1], rank);
            parent.SendServerMessage(ply.name + " set rank of " + cmdparts[1] + " to " + parent.PlyGetRank(cmdparts[1]));
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Sets rank of specified player to specified rank."; } }
        public override string Usage { get { return "<playername> <rankname>"; } }
    }
}
