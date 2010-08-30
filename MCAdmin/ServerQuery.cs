using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MCAdmin
{
    enum ServerQueryRequests
    {
        A2A_PING = 0x69,

        A2S_SERVERQUERY_GETCHALLENGE = 0x57,

        A2S_INFO = 0x54,
        A2S_PLAYER = 0x55,
        A2S_RULES = 0x56
    }

    enum ServerQueryResponses
    {
        A2A_PING = 0x6A,

        A2S_SERVERQUERY_GETCHALLENGE = 0x41,

        A2S_INFO = 0x49,
        A2S_PLAYER = 0x44,
        A2S_RULES = 0x45
    }

    class ServerQuery
    {
        public frmMain parent;
        public UdpClient externalListener;
        string ip; int port;
        public ServerQuery(frmMain m_parent)
        {
            parent = m_parent;
            ip = parent.GetServerProperty("server-ip-real", "0.0.0.0");
            port = Convert.ToInt32(parent.GetServerProperty("rcon-port", "25567"));
            externalListener = new UdpClient(new IPEndPoint(IPAddress.Parse(ip),port));
            receiverThread = new Thread(new ThreadStart(ReceiverThread));
            receiverThread.Start();
        }

        private byte[] GetPackBasic(ServerQueryResponses resp, int size)
        {
            byte[] pack = new byte[size];
            pack[0] = 255;
            pack[1] = 255;
            pack[2] = 255;
            pack[3] = 255;
            pack[4] = (byte)resp;
            return pack;
        }

        Thread receiverThread;
        void ReceiverThread()
        {
            byte[] buff;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);

            #region PING REPLY
            byte[] pingreply = GetPackBasic(ServerQueryResponses.A2A_PING, 25);
            System.Text.Encoding.ASCII.GetBytes("YIFF_YIFF_YIFF_YIFF").CopyTo(pingreply,5);
            #endregion

            #region INFO REPLY
            string sname = "MCAdmin Server";
            string smap = parent.GetServerProperty("world-name", "world");

            string sdir = "mcadmin";
            string sdesc = "Minecraft Alpha";
            string sver = "0.1.4";

            int inforeply_posplys = 0;
            byte[] inforeply = GetPackBasic(ServerQueryResponses.A2S_INFO, 21 + sname.Length + smap.Length + sdir.Length + sdesc.Length + sver.Length); //4 + 1 + 1 + STR (+1) + STR (+1) + FIXSTR (+1) + FIXSTR (+1) + 2 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + FIXSTR (+1) + 1 == 17 + STRING LEN

            int xpos = 5;

            inforeply[xpos] = 0x07;
            xpos++;
            
            System.Text.Encoding.ASCII.GetBytes(sname).CopyTo(inforeply, xpos);
            xpos += 1 + sname.Length;
            System.Text.Encoding.ASCII.GetBytes(smap).CopyTo(inforeply, xpos);
            xpos += 1 + smap.Length;
            System.Text.Encoding.ASCII.GetBytes(sdir).CopyTo(inforeply, xpos);
            xpos += 1 + sdir.Length;
            System.Text.Encoding.ASCII.GetBytes(sdesc).CopyTo(inforeply, xpos);
            xpos += 1 + sdesc.Length;

            BitConverter.GetBytes((short)9001).CopyTo(inforeply, xpos);
            xpos += 3; //2 for short, 1 for numplys of 0

            inforeply_posplys = xpos - 1;

            inforeply[xpos] = 24;
            xpos++;
            inforeply[xpos] = 0;
            xpos++;
            inforeply[xpos] = (byte)'d';
            xpos++;

            OperatingSystem os = Environment.OSVersion;
            int plat = (int)os.Platform;
            if (plat == 4 || plat == 128)
            {
                inforeply[xpos] = (byte)'l';
            }
            else
            {
                inforeply[xpos] = (byte)'w';
            }
            xpos++;

            inforeply[xpos] = 0;
            xpos++;
            inforeply[xpos] = (parent.GetServerProperty("online-mode", "true").ToLower() == "true") ? (byte)1 : (byte)0;
            xpos++;
            System.Text.Encoding.ASCII.GetBytes(sver).CopyTo(inforeply, xpos);
            xpos += 1 + sver.Length;
            inforeply[xpos] = 0;
            #endregion

            #region GETHCALLENGE REPLY
            byte[] challengereply = GetPackBasic(ServerQueryResponses.A2S_SERVERQUERY_GETCHALLENGE, 9);
            challengereply[5] = 0xCE;
            challengereply[6] = 0xDC;
            challengereply[7] = 0xED;
            challengereply[8] = 0x42;
            #endregion

            #region RULES REPLY
            byte[] playerreply = GetPackBasic(ServerQueryResponses.A2S_RULES, 1400);
            short rulecount = (short)(parent.serverProperties.Count - 1);
            xpos = 5;
            BitConverter.GetBytes(rulecount).CopyTo(playerreply, xpos);
            xpos += 2;
            foreach (KeyValuePair<string, string> kvp in parent.serverProperties)
            {
                if (kvp.Key == "rcon-pass") continue;
                System.Text.Encoding.ASCII.GetBytes(kvp.Key).CopyTo(playerreply, xpos);
                xpos += 1 + kvp.Key.Length;
                System.Text.Encoding.ASCII.GetBytes(kvp.Value).CopyTo(playerreply, xpos);
                xpos += 1 + kvp.Value.Length;
            }
            byte[] rulesreply = new byte[xpos];
            Array.Copy(playerreply, rulesreply, xpos);
            #endregion

            #region PLAYERS REPLY
            playerreply = GetPackBasic(ServerQueryResponses.A2S_PLAYER, 1400);
            #endregion

            while (true)
            {
                try
                {
                    buff = externalListener.Receive(ref endpoint);
                    if (buff[0] != 255 || buff[1] != 255 || buff[2] != 255 || buff[3] != 255) continue;
                    switch ((ServerQueryRequests)buff[4])
                    {
                        case ServerQueryRequests.A2A_PING:
                            externalListener.Send(pingreply, pingreply.Length, endpoint);
                            break;
                        case ServerQueryRequests.A2S_INFO:
                            if (buff[24] == 0 && System.Text.Encoding.ASCII.GetString(buff, 5, 19) == "Source Engine Query")
                            {
                                if (parent.minecraftFirewall != null) { inforeply[inforeply_posplys] = (byte)parent.minecraftFirewall.players.Count; }
                                else { inforeply[inforeply_posplys] = 0; }
                                externalListener.Send(inforeply, inforeply.Length, endpoint);
                            }
                            break;
                        case ServerQueryRequests.A2S_PLAYER:
                            if (buff[5] != 0xCE || buff[6] != 0xDC || buff[7] != 0xED || buff[8] != 0x42) { externalListener.Send(challengereply, challengereply.Length, endpoint); break; }
                            xpos = 5;
                            if (parent.minecraftFirewall != null) { playerreply[xpos] = (byte)parent.minecraftFirewall.players.Count; }
                            else { playerreply[xpos] = 0; }
                            
                            if(playerreply[xpos] <= 0) { externalListener.Send(playerreply, 6, endpoint); break; }
                            xpos++;

                            foreach (Player ply in parent.minecraftFirewall.players)
                            {
                                xpos++; //ID is always 0 :P
                                System.Text.Encoding.ASCII.GetBytes(ply.name).CopyTo(playerreply, xpos);
                                xpos += ply.name.Length;
                                playerreply[xpos] = 0;
                                xpos++;
                                BitConverter.GetBytes(ply.GetLevel()).CopyTo(playerreply, xpos);
                                xpos += 4;
                                playerreply[xpos] = 0;
                                playerreply[xpos + 1] = 0;
                                playerreply[xpos + 2] = 0;
                                playerreply[xpos + 3] = 0;
                                xpos += 4;
                            }

                            externalListener.Send(playerreply, xpos, endpoint);
                            break;
                        case ServerQueryRequests.A2S_RULES:
                            if (buff[5] != 0xCE || buff[6] != 0xDC || buff[7] != 0xED || buff[8] != 0x42) { externalListener.Send(challengereply, challengereply.Length, endpoint); break; }
                            externalListener.Send(rulesreply, rulesreply.Length, endpoint);
                            break;
                        case ServerQueryRequests.A2S_SERVERQUERY_GETCHALLENGE:
                            externalListener.Send(challengereply, challengereply.Length, endpoint);
                            break;
                    }
                }
                catch { }
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            try
            {
                externalListener.Close();
            }
            catch { }
            try
            {
                receiverThread.Abort();
            }
            catch { }
        }

        ~ServerQuery()
        {
            this.Dispose();
        }
    }
}
