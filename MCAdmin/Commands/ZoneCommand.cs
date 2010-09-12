using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class ZoneCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            switch (cmdparts[1].ToLower())
            {
                case "add":
                    break;
                case "delete":
                case "del":
                    Zone z = ply.FindApplicableZone();
                    if (z == null)
                    {
                        ply.SendDirectedMessage("You are in no zone!");
                    }
                    else
                    {
                        Program.zones.Remove(z);
                        ply.SendDirectedMessage("Zone deleted!");
                    }
                    break;
                case "setpriority":
                case "priority":
                    if (cmdparts.Length < 3) ply.SendDirectedMessage("Please specify the level!");
                    else
                    {
                        Zone z2 = ply.FindApplicableZone();
                        if (z2 == null)
                        {
                            ply.SendDirectedMessage("You are in no zone!");
                        }
                        else
                        {
                            z2.priority = Convert.ToInt32(cmdparts[2]);
                            ply.SendDirectedMessage("Set *zone* priority to: " + cmdparts[2]);
                        }
                    }
                    break;
                case "setlevel":
                case "level":
                    if (cmdparts.Length < 3) ply.SendDirectedMessage("Please specify the level!");
                    else
                    {
                        Zone z3 = ply.FindApplicableZone();
                        if (z3 == null)
                        {
                            Program.zoneDefaultLevel = Convert.ToInt32(cmdparts[2]);
                            ply.SendDirectedMessage("Set *default* build level to: " + cmdparts[2]);
                        }
                        else
                        {
                            z3.level = Convert.ToInt32(cmdparts[2]);
                            ply.SendDirectedMessage("Set *zone* build level to: " + cmdparts[2]);
                        }
                    }
                    break;
                default:
                    ply.SendDirectedMessage("Unknown zone command");
                    break;
            }
            Program.SaveZones();
        }

        public override int reqlevel { get { return 4; } }

        public override string Help { get { return "Zone management command"; } }
        public override string Usage { get { return "[add/delete/setlevel/setpriority] <level>"; } }
    }
}
