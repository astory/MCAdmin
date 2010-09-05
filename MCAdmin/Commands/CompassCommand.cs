using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin.Commands
{
    class CompassCommand : Command
    {
        public override void Run(Player ply, string[] cmdparts)
        {
            double rotation = ply.rot % 360.0;
            if (rotation < 0) rotation = 360 + rotation;
            double intdeg = rotation / 22.5;
            string rot;
            if (intdeg < 0.5)
            {
                rot = "W";
            }
            else if (intdeg < 1.5)
            {
                rot = "WNW";
            }
            else if (intdeg < 2.5)
            {
                rot = "NW";
            }
            else if (intdeg < 3.5)
            {
                rot = "NNW";
            }
            else if (intdeg < 4.5)
            {
                rot = "N";
            }
            else if (intdeg < 5.5)
            {
                rot = "NNE";
            }
            else if (intdeg < 6.5)
            {
                rot = "NE";
            }
            else if (intdeg < 7.5)
            {
                rot = "ENE";
            }
            else if (intdeg < 8.5)
            {
                rot = "E";
            }
            else if (intdeg < 9.5)
            {
                rot = "ESE";
            }
            else if (intdeg < 10.5)
            {
                rot = "SE";
            }
            else if (intdeg < 11.5)
            {
                rot = "SSE";
            }
            else if (intdeg < 12.5)
            {
                rot = "S";
            }
            else if (intdeg < 13.5)
            {
                rot = "SSW";
            }
            else if (intdeg < 14.5)
            {
                rot = "SW";
            }
            else if (intdeg < 15.5)
            {
                rot = "WSW";
            }
            else
            {
                rot = "W";
            }
            rotation -= 90;
            if (rotation < 0) rotation = 360 + rotation;
            ply.SendDirectedMessage("Direction: " + rot + " (" + ((int)Math.Round(rotation)) + " degrees)");
        }

        public override int reqlevel { get { return 0; } }

        public override string Help { get { return "Gives you your current bearing."; } }
        public override string Usage { get { return ""; } }
    }
}
