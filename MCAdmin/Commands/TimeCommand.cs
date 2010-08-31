using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class TimeCommand : Command
    {
        public TimeCommand(frmMain baseFrm)
        {
            parent = baseFrm;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length < 2 || cmdparts[1].ToLower() == "normal")
            {
                ply.forcedtime = -1;
                ply.SendChat("Reset your time back to normal!");
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
            if (settime < 0 || settime > 23) { ply.SendChat("Invalid time entered!"); return; }
            if (settime < 6)
            {
                ply.forcedtime = (18 + settime) * 1000;
            }
            else
            {
                ply.forcedtime = (settime - 6) * 1000;
            }
            ply.SendChat("Forced you time to be: " + settime.ToString() + ":00");
        }

        public override int reqlevel { get { return 1; } }

        public override string Help { get { return "Forces/fixes current time."; } }
        public override string Usage { get { return "<night/day/morning/afternoon/normal/0-23>"; } }
    }
}
