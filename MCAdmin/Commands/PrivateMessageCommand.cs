using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class PrivateMessageCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            try
            {
                Player ply2 = Program.minecraftFirewall.FindPlayer(cmdparts[1]);
                if (ply2 == null) { ply.SendDirectedMessage("Sorry, target could not be found!"); return; }

                var message = new StringBuilder();

                for (var i = 2; i < cmdparts.Length; i++)
                    message.Append(cmdparts[i] + " ");
                message.Remove(message.Length - 1, 1);

                Program.AddRTLine(Color.Black, "[PM] " + ply.name + " to " + ply2.name + ": " + message + "\n", true);
                ply.SendDirectedMessage("§e[PM >] §f" + ply2.name + "§f: " + message);
                ply2.SendDirectedMessage("§e[PM <] §f" + ply.name + "§f: " + message);
            }
            catch (Exception ex)
            {
                Program.SendLogMsg(ex.ToString());
            }

        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Sends a private message to specified player."; } }
        public override string Usage { get { return "<playername> <message>"; } }

    }
}