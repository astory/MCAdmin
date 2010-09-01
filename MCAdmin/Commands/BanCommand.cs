using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class BanCommand : Command
    {
        public BanCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() <= parent.PlyGetLevel(cmdparts[1])) { ply.SendPermissionDenied(); return; }
            Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { parent.PlySetRank(cmdparts[1], "banned"); parent.SendServerMessage(ply.name + " banned " + cmdparts[1]); return; }
            parent.PlySetRank(ply2.name, "banned");
            ply2.Disconnect("Kickbanned by " + ply.name);
            parent.SendServerMessage(ply.name + " kickbanned " + ply2.name);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Bans specified player"; } }
        public override string Usage { get { return "[playername]"; } }
    }
}
