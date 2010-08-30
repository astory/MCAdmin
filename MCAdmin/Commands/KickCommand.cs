using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class KickCommand : Command
    {
        public KickCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() < parent.PlyGetLevel(cmdparts[1])) { parent.SendPermissionDenied(ply); return; }
            Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { parent.SendDirectedMessage(ply, "Player could not be found!"); return; }
            ply2.Disconnect("Kicked by " + ply.name);
            parent.SendServerMessage(ply.name + " kicked " + ply2.name);
        }

        public override int reqlevel { get { return 2; } }

        public override string Help { get { return "Kicks specified player."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
