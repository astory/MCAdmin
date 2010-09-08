using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using MCAdmin.Commands;

namespace MCAdmin
{
    enum RconPacketType
    {
        SERVERDATA_RESPONSE_VALUE = 0,
        SERVERDATA_AUTH_RESPONSE = 2,

        SERVERDATA_AUTH = 3,
        SERVERDATA_EXECCOMMAND = 2
    }

    class RconConnection
    {
        Rcon rcon;
        Socket sock;
        bool authed;
        string ip;
        PlayerRcon ply;

        public RconConnection(Rcon m_rcon, Socket m_sock)
        {
            authed = false;
            sock = m_sock;
            sock.SendTimeout = 30000;
            sock.ReceiveTimeout = 30000;
            rcon = m_rcon;
            ip = IPAddress.Parse(((IPEndPoint)sock.RemoteEndPoint).Address.ToString()).ToString();
            ply = new PlayerRcon(ip, this);
            recvThread = new Thread(new ThreadStart(RecvThread));
            recvThread.Start();
            rcon.rconConnections.Add(this);
        }

        public void Disconnect()
        {
            try
            {
                recvThread.Abort();
            }
            catch { }

            try
            {
                sock.Close();
            }
            catch { }
            try
            {
                rcon.rconConnections.Remove(this);
            }
            catch { }
        }

        ~RconConnection()
        {
            this.Disconnect();
        }

        private IPEndPoint __TryGetIp(string ipport)
        {
            if (ipport[0] == '"') { ipport = ipport.Remove(ipport.Length - 1).Substring(1); }
            int psplit = ipport.IndexOf(':');
            if (psplit <= 0) throw new FormatException();
            return new IPEndPoint(IPAddress.Parse(ipport.Remove(psplit)),Convert.ToInt32(ipport.Substring(psplit+1)));
        }

        Thread recvThread;
        void RecvThread()
        {
            while (sock.Connected)
            {
                try
                {
                    RconPacket recvd = new RconPacket(this);
                    if (recvd.type == RconPacketType.SERVERDATA_AUTH)
                    {
                        if (recvd.string1 == rcon.password)
                        {
                            new RconPacket(this, RconPacketType.SERVERDATA_AUTH_RESPONSE, recvd.request_id, "", "").Send();
                            authed = true;
                        }
                        else
                        {
                            new RconPacket(this, RconPacketType.SERVERDATA_AUTH_RESPONSE, -1, "", "").Send();
                        }
                    }
                    else if (recvd.type == RconPacketType.SERVERDATA_EXECCOMMAND)
                    {
                        if (authed)
                        {
                            string response = "Unknown command or error!";
                            string cmd; string param;
                            string str = recvd.string1.Trim();
                            int splitter = str.IndexOf(' ');
                            if (splitter < 0)
                            {
                                cmd = str.ToLower();
                                param = "";
                            }
                            else
                            {
                                cmd = str.Remove(splitter).Trim().ToLower();
                                param = str.Substring(splitter + 1).Trim();
                            }
                            try
                            {
                                switch (cmd)
                                {
                                    case "help":
                                        response = "Commands: exec, stop, start, restart, kill, save, backup, status";
                                        break;
                                    case "exec":
                                        Program.SendServerCommand(param);
                                        response = "Ran command \"" + param + "\" on the server!";
                                        break;
                                    case "stop":
                                        Program.StopServer();
                                        response = "Server stopped!";
                                        break;
                                    case "start":
                                        Program.StartServer();
                                        response = "Server started!";
                                        break;
                                    case "restart":
                                        Program.StopServer();
                                        Program.StartServer();
                                        response = "Server restarted!";
                                        break;
                                    case "kill":
                                        Program.KillServer();
                                        response = "Server KILL'd!";
                                        break;
                                    case "save":
                                        Program.tmAutosave_Tick(null);
                                        response = "Save initiated!";
                                        break;
                                    case "backup":
                                        Program.tmBackup_Tick(null);
                                        response = "Backup initiated!";
                                        break;
                                    case "status":
                                        //response = Program.serverStatus;
                                        break;
                                    case "say":
                                        Program.SendServerMessage("RCon: " + param, '2');
                                        Program.SendLogMsg("\"Console<9999><Console><RCon>\" say \"" + param + "\"");
                                        break;

                                    case "logaddress_add":
                                        IPEndPoint ipep = __TryGetIp(param);
                                        if (!Program.logToAddr.Contains(ipep)) Program.logToAddr.Add(ipep);
                                        response = "Added " + param + " to log broadcast!";

                                        Program.SendLogMsg("rcon from \"" + ip + "\": command \"logaddress_add \"" + param + "\"\"\n");
                                        break;
                                    case "logaddress_del":
                                        IPEndPoint ipep2 = __TryGetIp(param);
                                        if (Program.logToAddr.Contains(ipep2)) Program.logToAddr.Remove(ipep2);
                                        response = "Removed " + param + " from log broadcast!";

                                        Program.SendLogMsg("rcon from \"" + ip + "\": command \"logaddress_del \"" + param + "\"\"\n");
                                        break;
                                    case "logaddress_list":
                                        response = "Log broadcast list:\n";
                                        foreach (IPEndPoint ipep3 in Program.logToAddr)
                                        {
                                            response += ipep3.ToString() + "\n";
                                        }
                                        break;
                                    case "logaddress_delall":
                                        Program.logToAddr.Clear();
                                        response = "Removed ALL IPs from log broadcast!";
                                        break;
                                    case "log":
                                        response = "Usage:  log < on | off >\ncurrently logging to: file, console, udp";
                                        break;

                                    default:
                                        if (Program.commands.ContainsKey(cmd))
                                        {
                                            ply.request_id = recvd.request_id;
                                            Program.commands[cmd].Run(ply, str.Split(new char[] {' '}));
                                            response = "";
                                        }
                                        break;
                                }
                            }
                            catch (Exception e) { response = "Command error: " + e.ToString(); }

                            if (response != null && response.Length > 0)
                            {
                                Program.AddRTLine(Color.Orange, "RCon from " + ip + ": " + recvd.string1 + "\r\n", true);
                                Program.AddRTLine(Color.Orange, "RCon response: " + response + "\r\n", true);

                                new RconPacket(this, RconPacketType.SERVERDATA_RESPONSE_VALUE, recvd.request_id, response + "\n", "").Send();
                            }
                        }
                        else
                        {
                            new RconPacket(this, RconPacketType.SERVERDATA_AUTH_RESPONSE, -1, "", "").Send();
                        }
                    }
                }
                catch { this.Disconnect(); return; }
                Thread.Sleep(10);
            }
        }

        internal class PlayerRcon : Player
        {
            RconConnection conn;
            public int request_id = 0;
            public PlayerRcon(string m_ip, RconConnection m_conn)
            {
                conn = m_conn;
                name = "Console";
                ip = m_ip;
            }

            public override void SendChat(string msg, bool doprefix, char colorCode)
            {
                new RconPacket(conn, RconPacketType.SERVERDATA_RESPONSE_VALUE, request_id, msg + "\n", "").Send();
            }

            public override void SendPacket(byte packet_id, byte[] data) { }

            public override void Disconnect(bool doprint) { }
            public override void Disconnect(string reason) { }

            public override void Disconnect() { }

            ~PlayerRcon() { }

            #region Player methods
            public override void ReadMsgFile(string file) { }

            public override bool GiveItem(string item, int amount)
            {
                return false;
            }

            public override bool GiveItem(int itemid, int amount)
            {
                return false;
            }

            public override string GetTag()
            {
                return "§2";
            }

            public override void SetRank(string rank) { }

            public override string GetRank()
            {
                return "Console";
            }

            public override int GetLevel()
            {
                return 9999;
            }

            public override bool HasLevel(int level)
            {
                return true;
            }
            #endregion
        }

        internal class RconPacket
        {
            private RconConnection conn;
            public RconPacketType type;
            public int request_id;
            public string string1;
            public string string2; //unused always lol

            public RconPacket(RconConnection m_conn, RconPacketType m_type, int m_request_id, string m_string1, string m_string2)
            {
                conn = m_conn;
                type = m_type;
                request_id = m_request_id;
                string1 = m_string1;
                string2 = m_string2;
            }

            public void Send()
            {
                int thissize = 10 + string1.Length + string2.Length;//4 + 4 + string (+1 because \0) + string (+1 because \0)
                byte[] thispacket = new byte[thissize + 4]; //+4 (packetsize)
                BitConverter.GetBytes(thissize).CopyTo(thispacket, 0);
                BitConverter.GetBytes(request_id).CopyTo(thispacket, 4);
                BitConverter.GetBytes((int)type).CopyTo(thispacket, 8);
                System.Text.Encoding.ASCII.GetBytes(string1, 0, string1.Length, thispacket, 12);
                System.Text.Encoding.ASCII.GetBytes(string2, 0, string2.Length, thispacket, 13 + string1.Length); //12 + string (+1 because \0)
                thispacket[thissize + 3] = 0;
                thispacket[12 + string1.Length] = 0;
                conn.sock.Send(thispacket);
            }

            public RconPacket(RconConnection m_conn)
            {
                conn = m_conn;
                int size = ReadInt();
                byte[] thispacket = ReadBytes(size);
                request_id = ReadInt(thispacket);
                type = (RconPacketType)ReadInt(thispacket);
                string1 = ReadString(thispacket);
                string2 = ReadString(thispacket);
            }

            #region Internal stuff
            private int tmppos = 0;

            private string ReadString(byte[] dat)
            {
                int endpos = tmppos;
                while (true)
                {
                    if (dat[endpos] == 0) break;
                    endpos++;
                }
                if (tmppos == endpos) { tmppos++; return ""; }
                else { string str = System.Text.Encoding.ASCII.GetString(dat, tmppos, endpos - tmppos); tmppos = endpos + 1; return str; }
            }

            private int ReadInt()
            {
                return BitConverter.ToInt32(ReadBytes(4), 0);
            }

            private int ReadInt(byte[] dat)
            {
                int ret = BitConverter.ToInt32(dat, tmppos);
                tmppos += 4;
                return ret;
            }

            private byte[] ReadBytes(int count)
            {
                byte[] buff = new byte[count];
                int recvd = 0; int left = count;
                while (left > 0)
                {
                    int recv = conn.sock.Receive(buff, recvd, left, SocketFlags.None);
                    recvd += recv;
                    left -= recv;
                    if(left > 0) Thread.Sleep(10);
                }
                return buff;
            }
            #endregion
        }
    }

    class Rcon
    {
        public List<RconConnection> rconConnections = new List<RconConnection>();
        TcpListener externalListener;
        string ip; int port; public string password;
        public Rcon()
        {
            ip = Program.GetServerProperty("server-ip-real", "0.0.0.0");
            port = Convert.ToInt32(Program.GetServerProperty("rcon-port", "25567"));
            password = Program.GetServerProperty("rcon-pass", "changeme");
            externalListener = new TcpListener(IPAddress.Parse(ip), port);
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
                        new RconConnection(this, externalListener.AcceptSocket());
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
            foreach (RconConnection conn in rconConnections.ToArray())
            {
                conn.Disconnect();
            }
        }
        ~Rcon()
        {
            this.Dispose();
        }
    }
}
