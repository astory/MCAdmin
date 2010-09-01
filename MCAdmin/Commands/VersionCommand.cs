using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class VersionCommand : Command
    {
        public VersionCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            ply.SendDirectedMessage("MCAdmin (c) 2010 by Doridian the blue haired fox :3");
        }

        public override int minlevel { get { return reqlevel; } set { } }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives name and author of this :3"; } }
        public override string Usage { get { return ""; } }
    }
}
