using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class KickCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (ply.GetLevel() < Program.PlyGetLevel(cmdparts[1])) { ply.SendPermissionDenied(); return; }

            string reasonstr = ""; string reason = "";
            if (cmdparts.Length > 2)
            {
                for (int i = 2; i < cmdparts.Length; i++)
                {
                    reason += cmdparts[i] + " ";
                }
                reason = reason.Remove(reason.Length - 1).Trim();
                reasonstr = reason;
            }


            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null)
            {
                ply2.Disconnect("Kicked by " + ply.name + ": No Reason Specified");
                Program.SendServerMessage(ply.name + " kicked " + cmdparts[1]);
            }
            else
            {
                ply2.Disconnect("Kicked by " + ply.name + ": " + reasonstr);
                Program.SendServerMessage(ply.name + " kicked " + ply2.name + " for " + reasonstr);
            }
            
        }

        public override int reqlevel { get { return 2; } }

        public override string Help { get { return "Kicks specified player."; } }
        public override string Usage { get { return "<playername> [reason]"; } }
    }
}
