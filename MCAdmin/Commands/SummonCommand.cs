using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class SummonCommand : Command
    {
        public SummonCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { ply.SendDirectedMessage("Sorry, target could not be found!"); return; }
            parent.SendServerCommand("tp " + ply2.name + " " + ply.name);
            parent.SendServerMessage(ply.name + " summoned " + ply2.name);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Teleports specified player to yourself."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
