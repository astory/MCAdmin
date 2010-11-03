/* Any contents of this document is not to be removed/deleted */
/* Permission is granted to add yourself, through the official developers are not to be removed */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class CreditsCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            ply.SendDirectedMessage("§9MCAdmin (c) 2010");
            ply.SendDirectedMessage("§a- §bDoridian: §fFounder & Main Blue Haired Fox Developer");
            ply.SendDirectedMessage("§a- §bToxicated: §fCombine Soldier Developer");
            ply.SendDirectedMessage("");
            ply.SendDirectedMessage("§eMCAdmin is released under the §cBSD License§e.");
        }

        public override int minlevel { get { return reqlevel; } set { } }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives the Credits & Developers for MCAdmin :3"; } }
        public override string Usage { get { return ""; } }
    }
}
