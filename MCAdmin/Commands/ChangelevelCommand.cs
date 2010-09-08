using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO;

namespace MCAdmin.Commands
{
    class ChangelevelCommand : Command
    {
        private void ChangeThread(object worldNameO)
        {
            Program.KillServer();
            Thread.Sleep(1000);
            try
            {
                Program.serverProperties["level-name"] = (string)worldNameO;
                Program.SaveServerProperties();
            }
            catch { }
            Program.StartServer();
            Program.isStuffInProgress = false;
        }

        public override void Run(Player ply, string[] cmdparts)
        {
            if (cmdparts.Length <= 1)
            {
                string str = "";
                foreach (string d in Directory.GetDirectories("."))
                {
                    if (File.Exists(d + "/level.dat"))
                    {
                        str += d.Substring(2) + ", ";
                    }
                }
                ply.SendDirectedMessage("Available levels: " + ((str.Length > 2) ? str.Remove(str.Length - 2) : "None"));
            }
            else if (Program.isStuffInProgress) ply.SendDirectedMessage("Sorry, not possible atm!");
            else
            {
                string worldName = cmdparts[1];
                if (File.Exists(worldName) && (!Directory.Exists(worldName) || !File.Exists(worldName + "/level.dat"))) { throw new InvalidOperationException(); }
                Program.SendServerMessage("Changing level from \"" + Program.GetServerProperty("level-name","world") + "\" to \"" + worldName + "\"");
                Program.isStuffInProgress = true;
                new Thread(new ParameterizedThreadStart(ChangeThread)).Start(worldName);
            }
        }

        public override int reqlevel { get { return 4; } }

        public override string Help { get { return "Changes the current level/world."; } }
        public override string Usage { get { return "<worldname>"; } }
    }
}
