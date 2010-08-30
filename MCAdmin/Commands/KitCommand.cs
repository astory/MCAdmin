using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class KitCommand : Command
    {
        public KitCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string strkits = "";
                int prank = ply.GetLevel();
                foreach (Kit k in parent.kits)
                {
                    if (prank >= k.reqlevel) strkits += k.name + ", ";
                }
                if (strkits.Length > 0) parent.SendDirectedMessage(ply, "Available kits: " + strkits.Remove(strkits.Length - 2));
                else parent.SendDirectedMessage(ply, "No kits available for you.");
            }
            else
            {
                Player giveto = ply;
                if (cmdparts.Length >= 3) giveto = parent.minecraftFirewall.FindPlayer(cmdparts[2]);
                if (giveto == null) { parent.SendDirectedMessage(ply, "Player not found!"); return; }
                string kitname = cmdparts[1].ToLower();
                Kit kit = null;
                foreach (Kit k in parent.kits)
                {
                    if (k.name == kitname)
                    {
                        kit = k;
                        break;
                    }
                }
                if (kit == null) { parent.SendDirectedMessage(ply, "Kit not found!"); return; }
                if (!ply.HasLevel(kit.reqlevel)) { parent.SendPermissionDenied(ply); return; }
                foreach (KeyValuePair<int, int> kv in kit.items)
                {
                    if (!giveto.GiveItem(kv.Key, kv.Value))
                    {
                        parent.SendDirectedMessage(ply, "Kit could not be given!");
                        break;
                    }
                }
            }
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Lists available kits or gives them to players (or yourself)"; } }
        public override string Usage { get { return "[kitname] [playername]"; } }
    }
}
