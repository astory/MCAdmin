using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MCAdmin.Heartbeats
{
    class MasterList
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


            byte[] data = System.Text.Encoding.ASCII.GetBytes(datastr);

            HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create("http://list.mcadmin.eu/announce.php");
            hwr.Proxy = null;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            hwr.ContentLength = data.Length;
            Stream str = hwr.GetRequestStream();
            str.Write(data, 0, data.Length);
            str.Close();
            HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
            if (hwres.StatusCode != HttpStatusCode.OK) { hwres.Close(); return; }
            str = hwres.GetResponseStream();
            StreamReader sr = new StreamReader(str);
            string response = sr.ReadToEnd();
            switch (response.ToLower())
            {
                case "ok":
                    
                    break;
                default:
                    Program.AddRTLine(System.Drawing.Color.Red, "Heartbeat fail: "+response+"!\r\n", true);
                    break;
            }
            sr.Close();
            str.Close();
            hwres.Close();
        }
    }
}
