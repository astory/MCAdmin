using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class UnbanipCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            string ip = cmdparts[1];
            Program.UnbanIP(ip);
            Program.SendServerMessage(ply.name + " un-ip-banned " + ip);
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Removes an IP-Ban for specified IP."; } }
        public override string Usage { get { return "<IP>"; } }
    }
}
