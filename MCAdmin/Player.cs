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
        
        //bool moddedServer = false;

        public bool frozen = false;
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
            bool invalidRecvd = false;
            byte[] dat = new byte[256];
            while (internalSock.Connected && externalSock.Connected)
            {
                try
                {
                    if (!invalidRecvd)
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
                                int packet_size = -2;
                                switch (packet_id)
                                {
                                    case 0x00:
                                        packet_size = 0;
                                        break;
                                    case 0x01:
                                        packet_size = -1;
                                        break;
                                    case 0x02:
                                        packet_size = -1;
                                        break;
                                    case 0x03:
                                        packet_size = -1;
                                        break;
                                    case 0x04:
                                        packet_size = 8; //8
                                        break;
                                    case 0x0A:
                                        packet_size = 1;
                                        break;
                                    case 0x0B:
                                        packet_size = 32; //8 + 8 + 8 + 8
                                        break;
                                    case 0x0C:
                                        packet_size = 8; //4 + 4
                                        break;
                                    case 0x0D:
                                        packet_size = 40; //8 + 8 + 8 + 8 + 4 + 4
                                        break;
                                    case 0x0E:
                                        packet_size = 11; //1 + 4 + 1 + 4 + 1
                                        break;
                                    case 0x0F:
                                        packet_size = 12; //2 + 4 + 1 + 4 + 1
                                        break;
                                    case 0x10:
                                        packet_size = 6; //4 + 2
                                        break;
                                    case 0x11:
                                        packet_size = 5; //2 + 1 + 2
                                        break;
                                    case 0x12:
                                        packet_size = 5; //4 + 1
                                        break;
                                    case 0x14:
                                        packet_size = -1;
                                        break;
                                    case 0x15:
                                        packet_size = 22; //4 + 2 + 1 + 4 + 4 + 4 + 1 + 1 + 1
                                        break;
                                    case 0x16:
                                        packet_size = 8; //4 + 4
                                        break;
                                    case 0x17:
                                        packet_size = 17; //4 + 1 + 4 + 4 + 4
                                        break;
                                    case 0x18:
                                        packet_size = 19; //4 + 1 + 4 + 4 + 4 + 1 + 1
                                        break;
                                    case 0x1D:
                                        packet_size = 4; //4
                                        break;
                                    case 0x1E:
                                        packet_size = 4; //4
                                        break;
                                    case 0x1F:
                                        packet_size = 7; //4 + 1 + 1 + 1
                                        break;
                                    case 0x20:
                                        packet_size = 6; //4 + 1 + 1
                                        break;
                                    case 0x21:
                                        packet_size = 9; //4 + 1 + 1 + 1 + 1 + 1
                                        break;
                                    case 0x22:
                                        packet_size = 18; //4 + 4 + 4 + 4 + 1 + 1
                                        break;
                                    case 0x32:
                                        packet_size = 9; //4 + 4 + 1
                                        break;
                                    case 0x33:
                                        packet_size = -1; //4 + 2 + 4 + 1 + 1 + 1 + 4 + ?
                                        break;
                                    case 0x34:
                                        packet_size = -1; //-1; //4 + 4 + 2 + ? + ? + ?
                                        break;
                                    case 0x35:
                                        packet_size = 11; //4 + 1 + 4 + 1 + 1
                                        break;
                                    case 0xFE:
                                        packet_size = -1;
                                        break;
                                    case 0xFF:
                                        packet_size = -1;
                                        break;
                                }
                                bool forwardpacket = true;
                                if (packet_size == -1)
                                {
                                    switch (packet_id)
                                    {
                                        case 0x01:
                                            ReceiveBytes(internalSock, 4); //4
                                            ReceiveString(internalSock);
                                            ReceiveString(internalSock);
                                            break;
                                        case 0x02:
                                            ReceiveString(internalSock);
                                            break;
                                        case 0x03:
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
                                        case 0xFF:
                                            this.Disconnect(ReceiveString(internalSock));
                                            break;
                                        case 0x14:
                                            ReceiveBytes(internalSock, 4); //4
                                            ReceiveString(internalSock);
                                            ReceiveBytes(internalSock, 16); //4 + 4 + 4 + 1 + 1 + 2
                                            break;
                                        case 0x33:
                                            ReceiveBytes(internalSock, 13); //4 + 2 + 4 + 1 + 1 + 1
                                            ReceiveBytes(internalSock, Util.AtoI(ReceiveBytes(internalSock, 4), 0));
                                            break;
                                        case 0x34:
                                            ReceiveBytes(internalSock, 8); //4 + 4
                                            ReceiveBytes(internalSock, Util.AtoN(ReceiveBytes(internalSock, 2), 0) * 4);
                                            break;
                                        /*case 0xFE:
                                            string type = ReceiveString(internalSock);
                                            string data = ReceiveString(internalSock);
                                            switch (type)
                                            {
                                                case "established":
                                                    moddedServer = true;
                                                    Program.AddRTLine(Color.Purple, "MODDED SERVER DETECTED!\r\n", false);
                                                    break;
                                                case "location":
                                                    break;
                                            }
                                            forwardpacket = false; //NEVER!
                                            break;*/
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
                                    Program.AddRTLine(Color.Orange, "Invalid packet ID: " + ((int)packet_id) + "\r\n", false);
                                    ReceiveBytes(internalSock, 1024);
                                    invalidRecvd = true;
                                }

                                if(forwardpacket) externalSock.Send(fwcache_int);
                            }
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        int recvd = internalSock.Receive(dat);
                        if(recvd > 0) externalSock.Send(dat, 0, recvd, SocketFlags.None);
                        else Thread.Sleep(10);
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
            bool invalidRecvd = false;
            byte[] dat = new byte[256];
            while (internalSock.Connected && externalSock.Connected)
            {
                try
                {
                    if (!invalidRecvd)
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
                                int packet_size = -2;
                                switch (packet_id)
                                {
                                    case 0x00:
                                        packet_size = 0;
                                        break;
                                    case 0x01:
                                        packet_size = -1;
                                        break;
                                    case 0x02:
                                        packet_size = -1;
                                        break;
                                    case 0x03:
                                        packet_size = -1;
                                        break;
                                    case 0x0A:
                                        packet_size = 1;
                                        break;
                                    case 0x0B:
                                        packet_size = 33; //8 + 8 + 8 + 8 + 1
                                        break;
                                    case 0x0C:
                                        packet_size = 9; //4 + 4 + 1
                                        break;
                                    case 0x0D:
                                        packet_size = 41; //8 + 8 + 8 + 8 + 4 + 4 + 1
                                        break;
                                    case 0x12: //see Server->Client 0x12
                                        packet_size = 5; //4 + 1
                                        break;
                                    case 0x0E: //see Server->Client 0x0E
                                        packet_size = 11; //1 + 4 + 1 + 4 + 1
                                        break;
                                    case 0x10: //Supposedly something to do with block placement (4 bytes [no idea?!], short [blocktype])
                                        packet_size = 6; //4 + 2
                                        break;
                                    case 0x0F: //see Server->Client 0x0F
                                        packet_size = 12; //2 + 4 + 1 + 4 + 1
                                        break;
                                    case 0x15: //see Server->Client 0x15
                                        packet_size = 22; //4 + 2 + 1 + 4 + 4 + 4 + 1 + 1 + 1
                                        break;
                                    /*case 0xFE:
                                        packet_size = -1;
                                        break;*/
                                    case 0xFF:
                                        packet_size = -1;
                                        break;
                                }
                                bool forwardpacket = true;
                                if (packet_size == -1)
                                {
                                    switch (packet_id)
                                    {
                                        case 0x01:
                                            ReceiveBytes(externalSock, 4);
                                            name = ReceiveString(externalSock);
                                            Program.AddRTLine(Color.Black, "IP " + this.ip + " logged in as " + name + "!\r\n", true);
                                            if (Util.ContainsInvalidChars(name, true)) { this.Disconnect("Don't use hax, fag :3"); return; }
                                            if (Program.PlyGetRank(name) == "banned") { this.Disconnect("You're banned"); return; }

                                            if (Program.mbansEnable && Program.masterBanList.Contains(name.ToLower())) { this.Disconnect("Globally banned. Visit http://bans.mcadmin.eu/?user=" + name); return; }

                                            Program.worldIsDirty = true;

                                            ReceiveString(externalSock);

                                            ReadMsgFile("welcome");

                                            break;
                                        case 0x02:
                                            string tmpnam = ReceiveString(externalSock);
                                            if (name != "" && tmpnam.ToLower() != name.ToLower()) { this.Disconnect("Don't use hax, fag :3"); return; }
                                            name = tmpnam;
                                            break;
                                        case 0x03:
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
                                                forwardpacket = true;
                                            }
                                            else
                                            {
                                                Program.AddRTLine(Color.Black, "<" + name + "> " + msg + "\r\n", true);
                                                Program.SendLogMsg("\"" + name + "<" + GetLevel() + "><" + name + "><" + GetRank() + ">\" say \"" + msg + "\"");
                                                string cmsg = GetTag() + this.name + ":§f " + msg;
                                                foreach(Player ply in fwl.players)
                                                {
                                                    ply.SendChat(cmsg);
                                                }
                                            }
                                            break;
                                        /*case 0xFE:
                                            this.Disconnect("Nope!");
                                            forwardpacket = false;
                                            break;*/
                                        case 0xFF:
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
                                            if (this.frozen)
                                            {
                                                Util.DinA(this.x, dat, 0);
                                                Util.DinA(this.y, dat, 8);
                                                Util.DinA(this.stance, dat, 16);
                                                Util.DinA(this.z, dat, 24);
                                                Util.FinA(this.rot, dat, 32);
                                                Util.FinA(this.pitch, dat, 36);
                                                this.SendPacket(0x0D, dat);
                                                /*switch (packet_id)
                                                {
                                                    case 0x0B:
                                                        Util.DinA(this.x, dat, 0);
                                                        Util.DinA(this.y, dat, 8);
                                                        Util.DinA(this.stance, dat, 16);
                                                        Util.DinA(this.z, dat, 24);
                                                        this.SendPacket(0x0B, dat);
                                                        break;
                                                    case 0x0C:
                                                        Util.FinA(this.rot, dat, 0);
                                                        Util.FinA(this.pitch, dat, 4);
                                                        this.SendPacket(0x0C, dat);
                                                        break;
                                                    case 0x0D:
                                                        
                                                        break;
                                                }*/
                                                forwardpacket = false;
                                            }
                                            else
                                            {
                                                __HandleMovementPacket(packet_id, dat, true);
                                            }
                                            break;
                                        case 0x0F:
                                            short blockid = Util.AtoN(dat, 0);
                                            if (blockid == 61) //now with working furnaces :3
                                            {
                                                blockid = 62;
                                            }
                                            else if (Program.blockLevels.ContainsKey(blockid))
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
                                            Util.NinA(blockid, dat, 0);
                                            break;
                                    }
                                    dat.CopyTo(fwcache_ext, 1);
                                }
                                if (packet_size == -2)
                                {
                                    Program.SendServerMessage("Client \"" + name + "\" (IP: "+ip+") sent unknown packet. Kicked!", '4');
                                    Program.AddRTLine(Color.Orange, "Invalid packet ID: " + ((int)packet_id) + "\r\n", false);
                                    //internalSock.Send(fwcache_ext);
                                    //invalidRecvd = true;
                                    //dat = new byte[256];
                                    this.Disconnect("Invalid packet ID: " + packet_id.ToString());
                                    return;
                                }
                                if (forwardpacket) internalSock.Send(fwcache_ext);
                            }
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        int recvd = externalSock.Receive(dat);
                        if (recvd > 0) internalSock.Send(dat, 0, recvd, SocketFlags.None);
                        else Thread.Sleep(10);
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
