using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class RulesCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            ply.ReadMsgFile("rules");
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Displays the Rules file"; } }
        public override string Usage { get { return ""; } }
    }
}
