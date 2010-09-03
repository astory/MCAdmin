using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class UnbanCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (Program.PlyGetRank(cmdparts[1]) != "banned") { ply.SendDirectedMessage("Player is not banned!"); return; }
            Program.PlySetRank(cmdparts[1], Program.GetServerProperty("default-rank","guest"));
            Program.SendServerMessage(ply.name + " unbanned " + cmdparts[1]);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Removes a ban for specified player."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
