using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing;

namespace MCAdmin.Commands
{
    class BackupCommand : Command
    {
        private void RestoreThread(object restoreNameO)
        {
            Program.KillServer();
            Thread.Sleep(1000);
            try
            {
                string fname = (string)restoreNameO;
                string wname = Program.GetServerProperty("level-name", "world");
                Program.AddRTLine(Color.Orange, "Restore in progress, removing old world...\r\n", true);
                Directory.Delete(wname, true);
                Program.AddRTLine(Color.Orange, "Restoring from " + fname + " now...\r\n", true);
                new FastZip().ExtractZip(fname, wname, "");
                Program.AddRTLine(Color.Orange, "Restore completed, restarting server!\r\n", true);
            }
            catch { }
            Program.StartServer();
            Program.isStuffInProgress = false;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            switch (cmdparts[1])
            {
                case "do":
                    Program.tmBackup_Tick(null);
                    ply.SendDirectedMessage("Backup initiated!");
                    break;
                case "list":
                    ply.SendDirectedMessage("Available backups for this world:");
                    foreach (string str in Directory.GetFiles("backups", Program.GetServerProperty("level-name", "world") + "_*.zip"))
                    {
                        ply.SendDirectedMessage(str.Substring(8,str.Length - 12));
                    }
                    break;
                case "restore":
                    string strf = "backups/" + cmdparts[2] + ".zip";
                    if (!File.Exists(strf)) ply.SendDirectedMessage("File not found!");
                    else if (Program.isStuffInProgress) ply.SendDirectedMessage("Sorry, restore in progress!");
                    else
                    {
                        Program.isStuffInProgress = true;
                        Program.SendServerMessage("Initiated backup restore. Prepare for disconnect!");
                        Thread.Sleep(1000);
                        new Thread(new ParameterizedThreadStart(RestoreThread)).Start(strf);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override int reqlevel { get { return 4; } }

        public override string Help { get { return "Manages backups."; } }
        public override string Usage { get { return "<do/list/restore> [file]"; } }
    }
}
