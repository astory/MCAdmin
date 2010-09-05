using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MCAdmin
{
    public class MCFirewall
    {
        public int intport = 0;
        TcpListener externalListener;
        public List<Player> players = new List<Player>();
        public long forcedtime = -1;

        public MCFirewall()
        {
            string ip = ""; int port = 0;
            ip = Program.GetServerProperty("server-ip-real","0.0.0.0");
            port = Convert.ToInt32(Program.GetServerProperty("server-port-real","25565"));
            intport = Convert.ToInt32(Program.GetServerProperty("server-port","25566"));

            Program.LoadBannedIPs();

            externalListener = new TcpListener(IPAddress.Parse(ip),port);
            externalListener.Server.SendTimeout = 500;
            externalListener.Server.ReceiveTimeout = 500;
            externalListener.Start(5);

            accepterThread = new Thread(new ThreadStart(AccepterThread));
            accepterThread.Start();
        }

        Thread accepterThread;
        void AccepterThread()
        {
            while (true)
            {
                try
                {
                    if (externalListener.Pending())
                    {
                        new Player(externalListener.AcceptSocket(), this);
                    }
                }
                catch { }
                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            try
            {
                accepterThread.Abort();
            }
            catch { }
            try
            {
                externalListener.Stop();
            }
            catch { }
            try
            {
                List<Player> tmpply = new List<Player>(players);
                foreach (Player ply in tmpply)
                {
                    try
                    {
                        ply.Disconnect();
                    }
                    catch { }
                }
            }
            catch { }
        }

        public void EnforceBans()
        {
            Player[] plys = players.ToArray();
            foreach (Player ply in plys)
            {
                if (Program.bannedIPs.Contains(ply.ip)) ply.Disconnect("You're banned!");
            }
        }

        public Player FindPlayer(string name)
        {
            if (name.Length <= 0) return null;
            Player result = null;
            name = name.ToLower();
            foreach (Player ply in players)
            {
                if (ply.name.ToLower().Contains(name))
                {
                    if (result != null) return null;
                    result = ply;
                }
            }
            return result;
        }

        ~MCFirewall()
        {
            this.Dispose();
        }
    }
}
