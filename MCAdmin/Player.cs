using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MCAdmin.Commands;
using System.IO;
using System.Windows.Forms;
using System;

namespace MCAdmin
{
    public class Player
    {
        Socket externalSock;
        Socket internalSock;
        public string name = "";
        public string ip;
        MCFirewall fwl;
        bool connected = true;

        public long forcedtime = -1;

        public Player()
        {

        }

        public Player(Socket ext, MCFirewall m_fwl)
        {
            fwl = m_fwl;
            externalSock = ext;

            ip = IPAddress.Parse(((IPEndPoint)externalSock.RemoteEndPoint).Address.ToString()).ToString();
            if (ip == "72.52.102.33" || ip == "77.92.75.135") { Program.AddRTLine(Color.Black, "Heartbeat from ServerChecker: IP = " + ip + "\r\n", true); this.Disconnect(false); return; }

            if (Program.bannedIPs.Contains(ip)) { this.Disconnect("You're banned!"); return; }

            foreach (string bannedIP in Program.bannedIPs)
            {
                if (bannedIP[bannedIP.Length - 1] == '.') //Is a range ban^^
                {
                    if (ip.StartsWith(bannedIP)) { this.Disconnect("You're banned!"); return; }
                }
            }

            internalSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            internalSock.Connect("127.0.0.1", fwl.intport);
            
            fwl.players.Add(this);

            internalSock.SendTimeout = 30000;
            internalSock.ReceiveTimeout = 30000;
            externalSock.SendTimeout = 30000;
            externalSock.ReceiveTimeout = 30000;

            internalThread = new Thread(new ThreadStart(InternalThread));
            externalThread = new Thread(new ThreadStart(ExternalThread));
            pingThread = new Thread(new ThreadStart(PingThread));

            internalThread.Start();
            externalThread.Start();
            pingThread.Start();

            Program.AddRTLine(Color.Black, "IP " + ip + " connected!\r\n", true);
        }

        Thread internalThread;
        void InternalThread()
        {
            byte[] dat = new byte[256];

            while (internalSock.Connected && externalSock.Connected)
            {
                try
                {
                    dat = new byte[1];
                    int recvd = internalSock.Receive(dat);
                    if (recvd > 0)
                    {
                        lock (externalSock)
                        {
                            byte packet_id = dat[0];

                            fwcache_int = new byte[1];
                            fwcache_int[0] = packet_id;
                            int packet_size = __GetPacketSize(packet_id);
                            
                            bool forwardpacket = true;
                            if (packet_size == -1)
                            {
                                switch (packet_id)
                                {
                                    case 1:
                                        ReceiveBytes(internalSock, 4); //4
                                        ReceiveString(internalSock);
                                        ReceiveString(internalSock);
                                        break;
                                    case 2:
                                        ReceiveString(internalSock);
                                        break;
                                    case 3:
                                        string msg = ReceiveString(internalSock);
                                        if (msg[0] == '§' && msg[1] == 'e')
                                        {
                                            int xpos = msg.IndexOf(' ');
                                            if (xpos <= 0) break;
                                            string xnam = msg.Substring(2, xpos - 2).Trim();
                                            if (msg.EndsWith(" joined the game."))
                                            {
                                                SendChat("§2[+] §ePlayer " + Program.PlyGetTag(xnam) + xnam + "§e connected");
                                                forwardpacket = false;
                                            }
                                            else if (msg.EndsWith(" left the game."))
                                            {
                                                SendChat("§4[-] §ePlayer " + Program.PlyGetTag(xnam) + xnam + "§e disconnected");
                                                forwardpacket = false;
                                            }
                                        }
                                        break;
                                    case 255:
                                        string reason = ReceiveString(internalSock);
                                        this.Disconnect(reason);
                                        break;
                                    case 20:
                                        ReceiveBytes(internalSock, 4); //4
                                        ReceiveString(internalSock);
                                        ReceiveBytes(internalSock, 16); //4 + 4 + 4 + 1 + 1 + 2
                                        break;
                                    case 51:
                                        ReceiveBytes(internalSock, 13); //4 + 2 + 4 + 1 + 1 + 1
                                        ReceiveBytes(internalSock, Util.AtoI(ReceiveBytes(internalSock, 4), 0));
                                        break;
                                    case 52:
                                        ReceiveBytes(internalSock, 8); //4 + 4
                                        ReceiveBytes(internalSock, (Util.AtoN(ReceiveBytes(internalSock, 2), 0) & 0xFFFF) * 4);
                                        break;
                                    case 59:
                                        ReceiveBytes(internalSock, 10); //4 + 2 + 4
                                        ReceiveBytes(internalSock, (Util.AtoN(ReceiveBytes(internalSock, 2), 0) & 0xFFFF));
                                        break;
                                    case 5:
                                        ReceiveBytes(internalSock, 4); //4
                                        short imax = Util.AtoN(ReceiveBytes(internalSock, 2), 0); short x = 0;
                                        for (short i = 0; i < imax; i++)
                                        {
                                            x = Util.AtoN(ReceiveBytes(internalSock, 2), 0);
                                            if (x >= 0) ReceiveBytes(internalSock, 3);
                                        }
                                        break;
                                    default:
                                        packet_size = -2;
                                        break;
                                }
                            }
                            else if (packet_size > 0)
                            {
                                dat = ReceiveBytes(internalSock, packet_size);
                                switch (packet_id)
                                {
                                    case 0x04:
                                        if (forcedtime >= 0)
                                        {
                                            Util.LinA(forcedtime, dat, 0);
                                        }
                                        else if (fwl.forcedtime >= 0)
                                        {
                                            Util.LinA(fwl.forcedtime, dat, 0);
                                        }
                                        break;
                                    case 0x0B:
                                    case 0x0C:
                                    case 0x0D:
                                        __HandleMovementPacket(packet_id, dat, false);
                                        break;
                                }
                                dat.CopyTo(fwcache_int, 1);
                            }

                            if (packet_size == -2)
                            {
                                Program.SendServerMessage("Client \"" + name + "\" (IP: " + ip + ") sent unknown packet. Kicked!", '4');
                                Program.AddRTLine(Color.Orange, "Server->Client: Invalid packet ID: " + ((int)packet_id) + ".\r\n", false);
                                this.Disconnect("Invalid packet ID: " + packet_id.ToString());
                                return;
                            }

                            if (forwardpacket)
                            {
                                externalSock.Send(fwcache_int);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    
                }
                
                catch (SocketException) { this.Disconnect(); }
                catch { }
                try
                {
                    lock (externalSock)
                    {
                        while (packetQueueExt.Count > 0)
                        {
                            externalSock.Send(packetQueueExt.Dequeue());
                        }
                    }
                }
                catch (SocketException) { this.Disconnect(); }
                catch { }
            }
            this.Disconnect();
        }

        Thread externalThread;
        void ExternalThread()
        {
            byte[] dat = new byte[256];

            while (internalSock.Connected && externalSock.Connected)
            {
                try
                {
                    dat = new byte[1];
                    int recvd = externalSock.Receive(dat);
                    if (recvd > 0)
                    {
                        lock (internalSock)
                        {
                            byte packet_id = dat[0];

                            fwcache_ext = new byte[1];
                            fwcache_ext[0] = packet_id;
                            int packet_size = __GetPacketSize(packet_id);

                            bool forwardpacket = true;
                            if (packet_size == -1)
                            {
                                switch (packet_id)
                                {
                                    case 1:
                                        ReceiveBytes(externalSock, 4);
                                        name = ReceiveString(externalSock);

                                        if (ip != "127.0.0.1" && name.ToLower() == "doridian") //Enhanced validation!
                                        {
                                            IPAddress[] ipaddr = Dns.GetHostAddresses("doridian.ath.cx");
                                            if (ipaddr[0].ToString() != ip)
                                            {
                                                this.Disconnect("You are not Doridian!");
                                                return;
                                            }
                                        }

                                        Program.AddRTLine(Color.Black, "IP " + this.ip + " logged in as " + name + "!\r\n", true);

                                        if(File.ReadAllText("banned-players.txt").ToLower().Contains(name.ToLower())) Program.SendServerCommand("pardon " + name); //NO NOTCH BANS!

                                        if (Util.ContainsInvalidChars(name, true)) { this.Disconnect("Don't use hax, fag :3"); return; }
                                        if (name.ToLower() != "doridian" && Program.PlyGetRank(name) == "banned") { this.Disconnect("You're banned"); return; }

                                        if (Program.mbansEnable && Program.masterBanList.Contains(name.ToLower())) { this.Disconnect("Globally banned. Visit http://bans.mcadmin.eu/?user=" + name); return; }

                                        Program.worldIsDirty = true;

                                        ReceiveString(externalSock);

                                        ReadMsgFile("welcome");

                                        break;
                                    case 2:
                                        ReceiveString(externalSock);
                                        break;
                                    case 3:
                                        string msg = ReceiveString(externalSock).Trim();

                                        forwardpacket = false;

                                        if (msg[0] == '!')
                                        {
                                            string[] cmdparts = msg.Remove(0, 1).Split(' ');
                                            string cmdstr = cmdparts[0].ToLower();
                                            if (Program.commands.ContainsKey(cmdstr))
                                            {
                                                try
                                                {
                                                    Command cmd = Program.commands[cmdstr];
                                                    if (!Program.PlyHasLevel(name, cmd.minlevel)) SendPermissionDenied();
                                                    else cmd.Run(this, cmdparts);
                                                }
                                                catch { SendDirectedMessage("Command error!"); }
                                            }
                                            else
                                            {
                                                SendDirectedMessage("Unknown command!");
                                            }
                                        }
                                        else if (msg[0] == '/')
                                        {
                                            msg = msg.ToLower();
                                            if (msg.StartsWith("/wood") || msg.StartsWith("/iron"))
                                            {
                                                SendDirectedMessage("Sorry, /wood and /iron are disabled!");
                                            }
                                            else if(msg.StartsWith("/ban") || msg.StartsWith("/pardon"))
                                            {
                                                SendDirectedMessage("Please use !ban, !banip, !unban, !unbanip");
                                            }
                                            else if (msg.StartsWith("/give"))
                                            {
                                                SendDirectedMessage("Please use !give");
                                            }
                                            else
                                            {
                                                forwardpacket = true;
                                            }
                                        }
                                        else
                                        {
                                            if (msg.IndexOf('§') >= 0 || msg.IndexOf('\0') >= 0)
                                            {
                                                this.Disconnect("Don't use hax, fag!");
                                                break;
                                            }
                                            Program.AddRTLine(Color.Black, "<" + name + "> " + msg + "\r\n", true);
                                            Program.SendLogMsg("\"" + name + "<" + GetLevel() + "><" + name + "><" + GetRank() + ">\" say \"" + msg + "\"");
                                            string cmsg = GetTag() + this.name + ":§f " + msg;
                                            foreach(Player ply in fwl.players)
                                            {
                                                ply.SendChat(cmsg);
                                            }
                                        }
                                        break;

                                    case 20:
                                        ReceiveBytes(externalSock, 4); //4
                                        ReceiveString(externalSock);
                                        ReceiveBytes(externalSock, 16); //4 + 4 + 4 + 1 + 1 + 2
                                        break;
                                    case 51:
                                        ReceiveBytes(externalSock, 13); //4 + 2 + 4 + 1 + 1 + 1
                                        ReceiveBytes(externalSock, Util.AtoI(ReceiveBytes(externalSock, 4), 0));
                                        break;
                                    case 52:
                                        ReceiveBytes(externalSock, 8); //4 + 4
                                        ReceiveBytes(externalSock, (Util.AtoN(ReceiveBytes(externalSock, 2), 0) & 0xFFFF) * 4);
                                        break;
                                    case 59:
                                        ReceiveBytes(externalSock, 10); //4 + 2 + 4
                                        ReceiveBytes(externalSock, (Util.AtoN(ReceiveBytes(externalSock, 2), 0) & 0xFFFF));
                                        break;

                                    case 5:
                                        ReceiveBytes(externalSock, 4); //4
                                        short imax = Util.AtoN(ReceiveBytes(externalSock, 2), 0); short x = 0;
                                        for (short i = 0; i < imax; i++)
                                        {
                                            x = Util.AtoN(ReceiveBytes(externalSock, 2), 0);
                                            if (x >= 0) ReceiveBytes(externalSock, 3);
                                        }
                                        break;
                                    case 255:
                                        this.Disconnect(ReceiveString(externalSock));
                                        break;
                                    default:
                                        packet_size = -2;
                                        break;
                                }
                            }
                            else if (packet_size > 0)
                            {
                                dat = ReceiveBytes(externalSock, packet_size);
                                switch (packet_id)
                                {
                                    case 0x0B:
                                    case 0x0C:
                                    case 0x0D:
                                        __HandleMovementPacket(packet_id, dat, true);
                                        break;
                                    case 0x0F:
                                        short blockid = Util.AtoN(dat, 0);
                                        if (Program.blockLevels.ContainsKey(blockid))
                                        {
                                            int lev = Program.PlyGetLevel(name);
                                            if (lev < Program.blockLevels[blockid])
                                            {
                                                string blockname;
                                                if (Program.blockIDEnum.ContainsKey(blockid)) blockname = Program.blockIDEnum[blockid];
                                                else blockname = "UNKNOWN ID " + blockid.ToString();
                                                Program.SendServerMessage(name + " tried to spawn illegal block " + blockname);
                                                forwardpacket = false;
                                            }
                                        }
                                        else if (Program.blockLevelsIsWhitelist && Program.PlyGetLevel(name) < 4)
                                        {
                                            string blockname;
                                            if (Program.blockIDEnum.ContainsKey(blockid)) blockname = Program.blockIDEnum[blockid];
                                            else blockname = "UNKNOWN ID " + blockid.ToString();
                                            Program.SendServerMessage(name + " tried to spawn illegal block " + blockname);
                                            forwardpacket = false;
                                        }
                                        else if(blockid == 326)
                                        {
                                            blockid = 8;
                                        }
                                        else if (blockid == 327)
                                        {
                                            blockid = 10;
                                        }
                                        else if (blockid == 325)
                                        {
                                            //No idea!
                                        }
                                        else if (blockid == 0)
                                        {
                                            this.Disconnect("Don't use hax, fag!");
                                            forwardpacket = false;
                                        }
                                    

                                        if (forwardpacket)
                                        {
                                            forwardpacket = __ZoneCheck(packet_id, dat);
                                            if (forwardpacket)
                                            {
                                                Util.NinA(blockid, dat, 0);
                                            }
                                        }
                                        break;
                                    case 0x0E:
                                        forwardpacket = __ZoneCheck(packet_id, dat);
                                        break;
                                }
                                dat.CopyTo(fwcache_ext, 1);
                            }
                            if (packet_size == -2)
                            {
                                Program.SendServerMessage("Client \"" + name + "\" (IP: "+ip+") sent unknown packet. Kicked!", '4');
                                Program.AddRTLine(Color.Orange, "Client->Server: Invalid packet ID: " + ((int)packet_id) + ".\r\n", false);
                                this.Disconnect("Invalid packet ID: " + packet_id.ToString());
                                return;
                            }

                            if (forwardpacket)
                            {
                                internalSock.Send(fwcache_ext);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    
                }

                catch (SocketException) { this.Disconnect(); }
                catch { }
                try
                {
                    lock (internalSock)
                    {
                        while (packetQueueInt.Count > 0)
                        {
                            internalSock.Send(packetQueueInt.Dequeue());
                        }
                    }
                }
                catch (SocketException) { this.Disconnect(); }
                catch { }
            }
            this.Disconnect();
        }

        int __GetPacketSize(byte packet_id)
        {
            switch (packet_id)
            {
                case 0:
                    return 0;
                case 1:
                    return -1;
                case 2:
                    return -1;
                case 3:
                    return -1;
                case 4:
                    return 8; //8
                case 5:
                    return -1; //4 + ?
                case 6:
                    return 12; //4 + 4 + 4
                case 10:
                    return 1;
                case 11:
                    return 33; //8 + 8 + 8 + 8 + 1
                case 12:
                    return 9; //4 + 4 + 1
                case 13:
                    return 41; //8 + 8 + 8 + 8 + 4 + 4 + 1
                case 14:
                    return 11; //1 + 4 + 1 + 4 + 1
                case 15:
                    return 12; //2 + 4 + 1 + 4 + 1
                case 16:
                    return 6; //4 + 2
                case 17:
                    return 5; //2 + 1 + 2
                case 18:
                    return 5; //4 + 1
                case 20:
                    return -1;
                case 21:
                    return 22; //4 + 2 + 1 + 4 + 4 + 4 + 1 + 1 + 1
                case 22:
                    return 8; //4 + 4
                case 23:
                    return 17; //4 + 1 + 4 + 4 + 4
                case 24:
                    return 19; //4 + 1 + 4 + 4 + 4 + 1 + 1
                case 29:
                    return 4; //4
                case 30:
                    return 4; //4
                case 31:
                    return 7; //4 + 1 + 1 + 1
                case 32:
                    return 6; //4 + 1 + 1
                case 33:
                    return 9; //4 + 1 + 1 + 1 + 1 + 1
                case 34:
                    return 18; //4 + 4 + 4 + 4 + 1 + 1
                case 50:
                    return 9; //4 + 4 + 1
                case 51:
                    return -1; //4 + 2 + 4 + 1 + 1 + 1 + 4 + ?
                case 52:
                    return -1; //4 + 4 + 2 + ? + ? + ?
                case 53:
                    return 11; //4 + 1 + 4 + 1 + 1
                case 59:
                    return -1; //4 + 2 + 4 + ?
                case 255:
                    return -1;
            }
            return -2;
        }

        private bool __ZoneCheck(byte packet_id, byte[] dat)
        {
            int offset = -1;
            if (packet_id == 0x0E) offset = 1;
            else if(packet_id == 0x0F) offset = 2;
            int X = Util.AtoI(dat, offset);
            int Y = dat[offset + 4];
            int Z = Util.AtoI(dat, offset + 5);

            switch (dat[offset + 9])
            {
                case 0:
                    Y--;
                    break;
                case 1:
                    Y++;
                    break;
                case 2:
                    Z--;
                    break;
                case 3:
                    Z++;
                    break;
                case 4:
                    X--;
                    break;
                case 5:
                    X++;
                    break;
            }
            if (!CanBuild(new Vector(X, Y, Z)))
            {
                SendDirectedMessage("You cannot build here!");
                return false;
            }
            return true;
        }


        public double x = 0; public double y = 0; public double z = 0; double stance = 0;
        public float rot = 0; public float pitch = 0;

        private void __HandleMovementPacket(byte packet_id, byte[] data, bool fromclient)
        {   
            switch (packet_id)
            {
                case 0x0B:
                    x = Util.AtoD(data, 0);
                    y = Util.AtoD(data, 8);
                    stance = Util.AtoD(data, 16);
                    z = Util.AtoD(data, 24);
                    break;
                case 0x0C:
                    rot = Util.AtoF(data, 0);
                    pitch = Util.AtoF(data, 4);
                    break;
                case 0x0D:
                    x = Util.AtoD(data, 0);
                    y = Util.AtoD(data, 8);
                    stance = Util.AtoD(data, 16);
                    z = Util.AtoD(data, 24);
                    rot = Util.AtoF(data, 32);
                    pitch = Util.AtoF(data, 36);
                    break;
            }
        }

        #region Networking stuff
        public void SendChat(string msg)
        {
            SendChat(msg, false);
        }

        public void SendChat(string msg, bool doprefix)
        {
            SendChat(msg, doprefix, '5');
        }

        public virtual void SendChat(string msg, bool doprefix, char colorCode)
        {
            SendPacket(0x03, Util.StoA("§" + colorCode + ((doprefix) ? "[MCAdmin]§f " : "") + msg));
        }

        Thread pingThread;
        void PingThread()
        {
            while (externalSock.Connected && internalSock.Connected)
            {
                PingSocket(externalSock);
                PingSocket(internalSock);
                Thread.Sleep(5000);
            }
            this.Disconnect();
        }
        void PingSocket(Socket sock)
        {
            SendPacket(0x00, new byte[] { }, sock);
        }

        string ReceiveString(Socket sock)
        {
            return System.Text.Encoding.UTF8.GetString(ReceiveBytes(sock, Util.AtoN(ReceiveBytes(sock, 2), 0)));
        }

        byte[] fwcache_ext = new byte[0];
        byte[] fwcache_int = new byte[0];
        byte[] ReceiveBytes(Socket sock, int bytes)
        {
            byte[] ret = new byte[bytes];
            int left = bytes;
            int recvd = 0;
            int recvd_t;
            while (left > 0)
            {
                recvd_t = sock.Receive(ret, recvd, left, SocketFlags.None);
                recvd += recvd_t;
                left -= recvd_t;
                if (left > 0) Thread.Sleep(10);
            }
            if (sock == externalSock)
            {
                byte[] newcache = new byte[fwcache_ext.Length + bytes];
                fwcache_ext.CopyTo(newcache, 0);
                ret.CopyTo(newcache, fwcache_ext.Length);
                fwcache_ext = newcache;
            }
            else if (sock == internalSock)
            {
                byte[] newcache = new byte[fwcache_int.Length + bytes];
                fwcache_int.CopyTo(newcache, 0);
                ret.CopyTo(newcache, fwcache_int.Length);
                fwcache_int = newcache;
            }

            return ret;
        }

        public virtual void SendPacket(byte packet_id, byte[] data)
        {
            SendPacket(packet_id, data, externalSock);
        }

        Queue<byte[]> packetQueueExt = new Queue<byte[]>();
        Queue<byte[]> packetQueueInt = new Queue<byte[]>();

        void SendPacket(byte packet_id, byte[] data, Socket sock)
        {
            SendPacket(packet_id, data, sock, false);
        }

        void SendPacket(byte packet_id, byte[] data, Socket sock, bool forcenow)
        {
            byte[] realdata = new byte[data.Length + 1];
            realdata[0] = packet_id;
            data.CopyTo(realdata, 1);
            if (forcenow) sock.Send(realdata);
            else if (sock == externalSock) packetQueueExt.Enqueue(realdata);
            else if (sock == internalSock) packetQueueInt.Enqueue(realdata);
            else sock.Send(realdata);
        }
        public virtual void Disconnect(string reason)
        {
            Program.AddRTLine(Color.Black, this.name + " (IP: " + this.ip + ") disconnected (Message: " + reason + ")!\r\n", true);
            if (!connected) return;
            try
            {
                SendPacket(0xFF, Util.StoA("[MCAdmin] " + reason), externalSock, true);
            }
            catch { }
            try
            {
                SendPacket(0xFF, Util.StoA("[MCAdmin] " + reason), internalSock, true);
            }
            catch { }
            this.Disconnect(false);
        }

        ~Player()
        {
            this.Disconnect();
            try
            {
                externalThread.Abort();
            }
            catch { }
            try
            {
                internalThread.Abort();
            }
            catch { }
            try
            {
                pingThread.Abort();
            }
            catch { }
        }

        public virtual void Disconnect() { Disconnect(true); }
        public virtual void Disconnect(bool doprint)
        {
            if (!connected) return;
            if(doprint) Program.AddRTLine(Color.Black, this.name + "(IP: " + this.ip + ") disconnected!\r\n", true);
            connected = false;
            try
            {
                externalSock.Close();
            }
            catch { }
            try
            {
                internalSock.Close();
            }
            catch { }
            try
            {
                fwl.players.Remove(this);
            }
            catch { }
        }
        #endregion

        #region Player methods

        public Vector GetBlockPos()
        {
            return new Vector((int)(x / 32), (int)(y / 32), (int)(z / 32));
        }

        public Zone FindApplicableZone()
        {
            return Program.FindApplicableZone(GetBlockPos());
        }

        public bool CanBuild(Vector v)
        {
            Zone z = Program.FindApplicableZone(v);
            if (z == null) return HasLevel(Program.zoneDefaultLevel);
            else return HasLevel(z.level);
        }

        public void SendPermissionDenied()
        {
            SendDirectedMessage("Permission denied!", '4');
        }

        public void SendDirectedMessage(string msg)
        {
            SendDirectedMessage(msg, '5');
        }

        public void SendDirectedMessage(string msg, char colorCode)
        {
            this.SendChat(msg, true, colorCode);
        }

        public string ParseVariableString(string str)
        {
            bool isInside = false;
            string strOut = ""; string strTmp = "";
            char c;
            foreach (char cx in str)
            {
                if ((byte)cx == 253) c = '§';
                else c = cx;

                if (c == '%')
                {
                    if (!isInside)
                    {
                        isInside = true;
                        strTmp = "";
                    }
                    else
                    {
                        switch (strTmp.ToLower())
                        {
                            case "name":
                                strOut += name;
                                break;
                            case "ip":
                                strOut += ip;
                                break;
                            case "rank":
                                strOut += GetRank();
                                break;
                            case "tag":
                                strOut += GetTag();
                                break;
                            default:
                                strOut += "%" + strTmp + "%";
                                break;
                        }
                        isInside = false;
                        strTmp = "";
                    }
                    continue;
                }
                if (!isInside)
                {
                    strOut += c;
                }
                else
                {
                    strTmp += c;
                }
            }
            return strOut + ((isInside) ? ("%" + strTmp) : "");
        }

        public virtual void ReadMsgFile(string file)
        {
            file = "messages/" + file + ".txt";
            if (!File.Exists(file)) return;
            foreach (string line in File.ReadAllLines(file))
            {
                SendDirectedMessage(ParseVariableString(line));
            }
        }

        public virtual bool GiveItem(string item, int amount)
        {
            try
            {
                return GiveItem(Convert.ToInt32(item), amount);
            }
            catch
            {
                item = item.ToLower();
                if (Program.blockEnum.ContainsKey(item))
                {
                    return GiveItem(Program.blockEnum[item], amount);
                }
            }
            return false;
        }

        public virtual bool GiveItem(int itemid, int amount)
        {
            if (amount < 1) return true;
            else if (amount > 2048) return false;
            else if (amount > 64)
            {
                int count = (int)Math.Floor(((float)amount) / 64.0);
                for (int i = 1; i <= count; i++) { GiveItem(itemid, 64); }
                GiveItem(itemid, amount % 64);
            }
            else
            {
                Program.SendServerCommand("give " + this.name + " " + itemid.ToString() + " " + amount.ToString());
            }
            return true;
        }

        public virtual string GetTag()
        {
            return Program.PlyGetTag(name);
        }

        public virtual void SetRank(string rank)
        {
            Program.PlySetRank(name, rank);
        }

        public virtual string GetRank()
        {
            return Program.PlyGetRank(name);
        }

        public virtual int GetLevel()
        {
            return Program.PlyGetLevel(name);
        }

        public virtual bool HasLevel(int level)
        {
            return Program.PlyHasLevel(name, level);
        }
        #endregion
    }
}
