namespace MCAdmin.Commands
{
    class NoTpCommand : Command
    {

        public override void Run(Player ply, string[] cmdparts)
        {
            ply.allowTP = !ply.allowTP;
            ply.SendDirectedMessage("Incoming teleports will be " + (ply.allowTP ? "accepted." : "rejected."));
        }

        public override int reqlevel { get { return 1; } }

        public override string Help { get { return "Toggles whether or not you can be teleported to."; } }
        public override string Usage { get { return ""; } }
    }
}