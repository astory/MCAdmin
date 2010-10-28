using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class TellCommand : Command
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

                Program.AddRTLine(Color.Black, ply.name + " whispered to " + ply2.name + ": " + message + "\n", true );
                ply.SendDirectedMessage("You whisper to " + ply2.name + ".");
                ply2.SendDirectedMessage(ply.name + " whispers: " + message );
            }
            catch (Exception ex)
            {
                Program.SendLogMsg(ex.ToString());
            }

        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Sends a private message to specified player."; } }
        public override string Usage { get { return "<playername>"; } }

    }
}