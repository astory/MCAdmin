using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class ServertimeCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length < 2 || cmdparts[1].ToLower() == "normal")
            {
                Program.minecraftFirewall.forcedtime = -1;
                Program.SendServerMessage(ply.name + " reset time back to normal!");
                return;
            }
            int settime = -1;
            switch (cmdparts[1].ToLower())
            {
                case "night":
                    settime = 0;
                    break;
                case "day":
                    settime = 12;
                    break;
                case "morning":
                    settime = 6;
                    break;
                case "afternoon":
                    settime = 18;
                    break;
                default:
                    try
                    {
                        settime = Convert.ToInt32(cmdparts[1]);
                    }
                    catch { }
                    break;
            }
            if (settime < 0 || settime > 23) { ply.SendDirectedMessage("Invalid time entered!"); return; }
            if (settime < 6)
            {
                Program.minecraftFirewall.forcedtime = (18 + settime) * 1000;
            }
            else
            {
                Program.minecraftFirewall.forcedtime = (settime - 6) * 1000;
            }
            Program.SendServerMessage(ply.name + " forced time to be: " + settime.ToString() + ":00");
        }

        public override int reqlevel { get { return 3; } }

        public override string Help { get { return "Forces/fixes current time *serverside*."; } }
        public override string Usage { get { return "<night/day/morning/afternoon/normal/0-23>"; } }
    }
}
