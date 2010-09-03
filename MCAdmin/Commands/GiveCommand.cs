using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class GiveCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            Player giveto = ply;
            int amt = 1;
            if (cmdparts.Length >= 4) giveto = Program.minecraftFirewall.FindPlayer(cmdparts[3]);
            if (cmdparts.Length >= 3) amt = Convert.ToInt32(cmdparts[2]);
            if (!giveto.GiveItem(cmdparts[1].Replace('_', ' '), amt))
                ply.SendDirectedMessage("Item could not be given!");
            else
                ply.SendDirectedMessage("Item given successfully!"); ;
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Gives an item to specified player (or yourself)"; } }
        public override string Usage { get { return "<Item name or ID> [amount] [playername]"; } }
    }
}
