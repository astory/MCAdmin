using System;
using System.Collections.Generic;
using System.Text;

namespace MCAdmin.Commands
{
    class TpCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { ply.SendDirectedMessage("Sorry, target could not be found!"); return; }

            if (!ply2.allowTP && ply.GetLevel() <= ply2.GetLevel()) { ply.SendDirectedMessage("Sorry, target has disallowed incoming teleports!"); return; }

            Program.SendServerCommand("tp " + ply.name + " " + ply2.name);
            Program.SendServerMessage(ply.name + " teleported to " + ply2.name);
        }

        public override int reqlevel { get { return 1; } }

        public override string Help { get { return "Teleports yourself to specified player."; } }
        public override string Usage { get { return "<playername>"; } }
    }
}
