using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class FreezeCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null) { ply.SendDirectedMessage("Sorry, target could not be found!"); return; }
            if (cmdparts.Length < 3 || cmdparts[2].ToLower() == "on")
            {
                ply2.frozen = true;
                Program.SendServerMessage(ply.name + " froze " + ply2.name + "!");
            }
            else
            {
                ply2.frozen = false;
                Program.SendServerMessage(ply.name + " un-froze " + ply2.name + "!");
            }
        }

        public override int reqlevel { get { return 4; } }

        public override string Help { get { return "Freezes the player in position, or unfreezes."; } }
        public override string Usage { get { return "<playername> <on/off>"; } }
    }
}
