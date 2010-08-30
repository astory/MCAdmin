using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class UnbanCommand : Command
    {
        public UnbanCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (parent.PlyGetRank(cmdparts[1]) != "banned") { parent.SendDirectedMessage(ply, "Player is not banned!"); return; }
            parent.PlySetRank(cmdparts[1], parent.GetServerProperty("default-rank","guest"));
            parent.SendServerMessage(ply.name + " unbanned " + cmdparts[1]);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Removes a ban for specified player."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
