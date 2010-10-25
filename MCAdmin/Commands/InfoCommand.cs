using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class InfoCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            ply.SendDirectedMessage("§9" + Program.GetServerProperty("server-name", "MCAdmin Server"));
            ply.SendDirectedMessage("§eServer Address: §f" + Program.externalIP + ":" + Program.GetServerProperty("server-port-real", "25565"));
            ply.ReadMsgFile("info");
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Displays Information about the server aswell as info from the info file"; } }
        public override string Usage { get { return ""; } }
    }
}