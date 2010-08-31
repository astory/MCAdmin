using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class FreezeCommand : Command
    {
        public FreezeCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            Player ply2 = parent.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { parent.SendDirectedMessage(ply, "Sorry, no player found!"); return; }
            if (cmdparts.Length < 3 || cmdparts[2].ToLower() == "on")
            {
                ply2.frozen = true;
                parent.SendServerMessage(ply.name + " froze " + ply2.name + "!");
            }
            else
            {
                ply2.frozen = false;
                parent.SendServerMessage(ply.name + " un-froze " + ply2.name + "!");
            }
        }

        public override int reqlevel { get { return 4; } }

        public override string Help { get { return "Freezes the player in position, or unfreezes."; } }
        public override string Usage { get { return "<playername> <on/off>"; } }
    }
}
