using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MCAdmin.Heartbeats
{
    static class MasterList
    {
        public static void Pump()
        {
            string datastr = "";
            foreach (KeyValuePair<string, string> kvp in Program.serverProperties)
            {
                if (kvp.Key == "rcon-pass") continue;
                datastr += kvp.Key + "=" + kvp.Value + "&";
            }
            int plys = ((Program.serverFullyOnline && Program.minecraftFirewall != null) ? Program.minecraftFirewall.players.Count : 0);
            datastr += "status="+Program.serverFullyOnline.ToString()+"&player-count="+plys+"&players=";
            if (plys > 0 && Program.mlistSendNames)
            {
                foreach (Player ply in Program.minecraftFirewall.players)
                {
                    datastr += ply.name + "," + ((Program.mlistSendRanks) ? ply.GetRank() : "") + ";";
                }
                datastr = datastr.Remove(datastr.Length - 1);
            }

            string response = PostRequest.Send("https://list.mcadmin.eu/announce.php", datastr);

            switch (response.ToLower())
            {
                case "ok":
                    
                    break;
                default:
                    Program.AddRTLine(System.Drawing.Color.Red, "Heartbeat fail: "+response+"!\r\n", true);
                    break;
            }
        }
    }
}
