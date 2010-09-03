using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MCAdmin.Commands
{
    class BanipCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            string ip = "";
            Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
            if (ply2 != null)
            {
                if (ply.GetLevel() <= ply2.GetLevel()) { ply.SendPermissionDenied(); return; }
                ip = ply2.ip;
            }
            else
            {
                IPAddress xip;
                if (!IPAddress.TryParse(cmdparts[1], out xip))
                {
                    ply.SendDirectedMessage("Player not found and parameter is no valid IP!");
                    return;
                }
                ip = xip.ToString();
            }
            Program.SendServerMessage(ply.name + " ip-banned " + ip);
            Program.BanIP(ip);
            Program.minecraftFirewall.EnforceBans();
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "IP-Bans specified IP (or player's IP if they are online)."; } }
        public override string Usage { get { return "<playername or IP>"; } }
    }
}
