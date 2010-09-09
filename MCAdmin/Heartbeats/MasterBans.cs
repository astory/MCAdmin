using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Drawing;

namespace MCAdmin.Heartbeats
{
    static class MasterBans
    {
        public static void Pump()
        {
            string[] banlist = PostRequest.Send("https://bans.mcadmin.eu/uplink_list.php", "validation=hot_382_gay_3848_fox_5832_yiff").Split(new char[] {':'});
            Program.masterBanList.Clear();
            foreach(string ban in banlist)
            {
                if(ban.Length > 0) Program.masterBanList.Add(ban.ToLower());
            }
        }

        struct BanStruct
        {
            public string name;
            public string admin;
            public string reason;
            public string action;
        }

        public static void BanUser(string name, string admin, string reason)
        {
            if (!Program.mbansSubmit || !Program.mbansEnable) return;
            if (Program.PlyGetRank(name) != "banned") return;

            BanStruct bstr = new BanStruct();
            bstr.action = "ban";
            bstr.name = name;
            bstr.admin = admin;
            bstr.reason = reason;
            new Thread(new ParameterizedThreadStart(__BanUser)).Start(bstr);
        }

        public static void UnbanUser(string name, string admin)
        {
            if (!Program.mbansSubmit || !Program.mbansEnable) return;
            if (Program.PlyGetRank(name) == "banned") return;

            BanStruct bstr = new BanStruct();
            bstr.action = "unban";
            bstr.name = name;
            bstr.admin = admin;
            bstr.reason = "";
            new Thread(new ParameterizedThreadStart(__BanUser)).Start(bstr);
        }

        static void __BanUser(object bs)
        {
            BanStruct bstr = (BanStruct)bs;
            string resp = PostRequest.Send("https://bans.mcadmin.eu/uplink.php", "action=" + bstr.action + "&name=" + bstr.name + "&admin=" + bstr.admin + "&port=" + Program.GetServerProperty("server-port-real","25565") + "&reason=" + HttpUtility.UrlEncode(bstr.reason) + "&validation=" + __GetValidation(bstr));
            if(resp.ToLower() != "ok") Program.AddRTLine(Color.Red, "Heartban fail: " + resp + "\r\n", true);
        }

        static string __GetValidation(BanStruct bstr)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            string temp = "";
            string result = "";
            byte[] res = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes("wizzy_" + bstr.action + "_i_" + bstr.name + "_love_" + bstr.admin + "_you_" + bstr.reason + "_forever_" + Program.externalIP + "_!"));
            for (int i = 0; i < res.Length; i++)
            {
                temp = Convert.ToString(res[i], 16);
                if (temp.Length == 1)
                    temp = "0" + temp;
                result += temp;
            }
            return result;
        }
    }
}
