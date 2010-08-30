using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class TpCommand : Command
    {
        public TpCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { parent.SendDirectedMessage(ply, "Sorry, target could not be found!"); return; }
            parent.SendServerCommand("tp " + ply.name + " " + ply2.name);
            parent.SendServerMessage(ply.name + " teleported to " + ply2.name);
        }

        public override int reqlevel { get { return 1; } }

        public override string Help { get { return "Teleports yourself to specified player."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
