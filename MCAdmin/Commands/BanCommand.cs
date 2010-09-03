using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class BanCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() <= Program.PlyGetLevel(cmdparts[1])) { ply.SendPermissionDenied(); return; }
            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { Program.PlySetRank(cmdparts[1], "banned"); Program.SendServerMessage(ply.name + " banned " + cmdparts[1]); return; }
            Program.PlySetRank(ply2.name, "banned");
            ply2.Disconnect("Kickbanned by " + ply.name);
            Program.SendServerMessage(ply.name + " kickbanned " + ply2.name);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Bans specified player"; } }
        public override string Usage { get { return "[playername]"; } }
    }
}
