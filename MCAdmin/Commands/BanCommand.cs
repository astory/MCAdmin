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

            string reasonstr = ""; string reason = "";
            if (cmdparts.Length > 2)
            {
                for (int i = 2; i < cmdparts.Length; i++)
                {
                    reason += cmdparts[i] + " ";
                }
                reason = reason.Remove(reason.Length - 1).Trim();
                reasonstr = " (" + reason + ")";
            }

            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 == null)
            {
                Program.PlySetRank(cmdparts[1], "banned");
                Program.SendServerMessage(ply.name + " banned " + cmdparts[1]);
            }
            else
            {
                Program.PlySetRank(ply2.name, "banned");
                ply2.Disconnect("Kickbanned by " + ply.name + reasonstr);
                Program.SendServerMessage(ply.name + " kickbanned " + ply2.name + reasonstr);
            }

            if (reason != "") Heartbeats.MasterBans.BanUser(cmdparts[1], ply.name, reason);
            else if (Program.mbansEnable && Program.mbansSubmit)
            {
                ply.SendDirectedMessage("§4Global Ban Submission Declined!");
                ply.SendDirectedMessage("§ePlease reban again with a reason (!ban name reason)!");
            }

            if (Program.PlyIsDev(cmdparts[1])) ply.SendDirectedMessage("§4You just BANNED an Official MCAdmin Developer!");
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Bans specified player"; } }
        public override string Usage { get { return "<playername> [reason]"; } }
    }
}
