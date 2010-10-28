using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class VersionCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            ply.SendDirectedMessage("§9MCAdmin (c) 2010");
            ply.SendDirectedMessage("§eBy Doridian the Blue Haired Fox");
            ply.SendDirectedMessage("");
            ply.SendDirectedMessage("§eYou are not allowed to edit MCAdmin in any way, unless you have permission");
            ply.SendDirectedMessage("§efrom Doridian. MCAdmin is not Open Source.");
        }

        public override int minlevel { get { return reqlevel; } set { } }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives Name and Author of this :3"; } }
        public override string Usage { get { return ""; } }
    }
}
