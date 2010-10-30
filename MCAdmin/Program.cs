using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using MCAdmin.Commands;
using Microsoft.Win32;
using System.Reflection;

namespace MCAdmin
{
    public static class Program
    {   
        public static frmMain mainFrm;
        public static bool dontUpdate = false;
        public static bool dontUpdateMCAdmin = false;
        public static bool dontUpdateJAR = false;
        public static bool consoleOnly = false;
        public static bool autoStartServer = false;

        #region Header variables
        public static bool isStuffInProgress = false;

        static bool isOutOfDate_MCA = false;
        static bool isOutOfDate_JAR = false;

        public static List<IPEndPoint> logToAddr = new List<IPEndPoint>();
        public static Rcon serverRcon;
        public static ServerQuery serverQuery;

        public static Thread updaterThread;
        public static Process minecraftServer;

        public static Dictionary<string, string> plyranks = new Dictionary<string, string>();
        public static Dictionary<string, int> ranklevels = new Dictionary<string, int>();
        public static Dictionary<string, string> ranktags = new Dictionary<string, string>();

        public static Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public static Dictionary<string, int> blockEnum = new Dictionary<string, int>();
        public static Dictionary<int, string> blockIDEnum = new Dictionary<int, string>();

        static bool updateRunning = false;

        public static string javaExecutable;

        public static bool serverFullyOnline;
        public static string serverStatus = "Stopped";

        public static MCFirewall minecraftFirewall;

        public static bool worldIsDirty = false;

        static System.Threading.Timer tmBackup;
        static System.Threading.Timer tmAutosave;
        static System.Threading.Timer tmCheckUpdate;

        static System.Threading.Timer tmHeartbeat;

        public static string externalIP = "1.3.3.7";
        #endregion

        #region MasterList management
        public static List<string> masterBanList = new List<string>();

        public static bool mlistEnable = true;
        public static bool mlistSendNames = true;
        public static bool mlistSendRanks = true;

        public static bool mbansEnable = true;
        public static bool mbansSubmit = true;

        public static void LoadMainConfig()
        {
            if (File.Exists("main-config.txt"))
            {
                string[] lines = File.ReadAllLines("main-config.txt");
                char[] splitter = new char[] { '=' };
                foreach (string line in lines)
                {
                    string[] infos = line.ToLower().Split(splitter, 2);
                    if (infos.Length != 2) continue;
                    bool val = (infos[1] == "true");
                    switch (infos[0].ToLower())
                    {
                        case "list-enable":
                            mlistEnable = val;
                            break;
                        case "list-send-names":
                            mlistSendNames = val;
                            break;
                        case "list-send-ranks":
                            mlistSendRanks = val;
                            break;
                        case "bans-enable":
                            mbansEnable = val;
                            break;
                        case "bans-submit":
                            mbansSubmit = val;
                            break;
                    }
                }
            }
            else if(!consoleOnly)
            {
                mainFrm.BeginInvoke(new MethodInvoker(delegate() {
                    new frmWelcome().ShowDialog(mainFrm);
                }));
            }
        }

        public static void SaveMainConfig()
        {
            FileStream fs = File.Open("main-config.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("list-enable=" + mlistEnable.ToString());
            sw.WriteLine("list-send-names=" + mlistSendNames.ToString());
            sw.WriteLine("list-send-ranks=" + mlistSendRanks.ToString());

            sw.WriteLine("bans-enable=" + mbansEnable.ToString());
            sw.WriteLine("bans-submit=" + mbansSubmit.ToString());

            sw.Close();
            fs.Close();
        }
        #endregion

        #region Ranks level management
        public static void LoadRankLevels()
        {
            ranklevels.Clear();
            ranktags.Clear();

            if (!File.Exists("ranks-config.txt"))
            {
                ranklevels.Add("guest", 0);
                ranklevels.Add("banned", -1);
                ranklevels.Add("builder", 1);
                ranklevels.Add("advbuilder", 2);
                ranklevels.Add("op", 3);
                ranklevels.Add("admin", 4);

                ranktags.Add("banned", "§0");
                ranktags.Add("guest", "§7");
                ranktags.Add("builder", "§a");
                ranktags.Add("advbuilder", "§2");
                ranktags.Add("op", "§b");
                ranktags.Add("admin", "§5");
            }
            else
            {
                string[] lines = File.ReadAllLines("ranks-config.txt");
                char[] splitter = new char[] { '=' };
                foreach (string line in lines)
                {
                    string[] infos = line.Split(splitter, 3);
                    if (infos.Length != 3) continue;
                    ranklevels.Add(infos[0], Convert.ToInt32(infos[1]));
                    ranktags.Add(infos[0], infos[2]);
                }
            }
        }

        public static void SaveRankLevels()
        {
            FileStream fs = File.Open("ranks-config.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            string tmptag = "";
            foreach (KeyValuePair<string, int> kvp in ranklevels)
            {
                if (ranktags.ContainsKey(kvp.Key)) tmptag = ranktags[kvp.Key];
                else tmptag = "";
                sw.WriteLine(kvp.Key + "=" + kvp.Value + "=" + tmptag);
            }
            sw.Close();
            fs.Close();
        }
        #endregion

        #region Command Level management
        public static void LoadCommandLevels()
        {
            if (File.Exists("command-levels.txt"))
            {
                string[] lines = File.ReadAllLines("command-levels.txt");
                int pos1; string bidx; int blvl;
                foreach (string line in lines)
                {
                    pos1 = line.IndexOf('=');
                    if (pos1 <= 0) continue;
                    bidx = line.Remove(pos1);
                    blvl = Convert.ToInt32(line.Substring(pos1 + 1));
                    if (commands.ContainsKey(bidx)) commands[bidx].minlevel = blvl;
                }
            }
        }

        public static void SaveCommandLevels()
        {
            FileStream fs = File.Open("command-levels.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (KeyValuePair<string, Command> kvp in commands)
            {
                sw.WriteLine(kvp.Key.ToString() + '=' + kvp.Value.minlevel.ToString());

            }
            sw.Close();
            fs.Close();
        }
        #endregion

        #region Block-List mangement
        public static bool blockLevelsIsWhitelist = false;
        public static Dictionary<int, int> blockLevels = new Dictionary<int, int>();

        public static void SaveBlockList()
        {
            FileStream fs = File.Open("blocks-list.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("iswhitelist=" + ((blockLevelsIsWhitelist) ? '1' : '0'));
            foreach (KeyValuePair<int, int> kvp in blockLevels)
            {
                sw.WriteLine(kvp.Key.ToString() + '=' + kvp.Value.ToString());
            }
            sw.Close();
            fs.Close();
        }

        public static void LoadBlockList()
        {
            blockLevels.Clear();
            if (!File.Exists("blocks-list.txt"))
            {
                blockLevels.Add(7, 4); //adminium

                blockLevels.Add(8, 2); //water

                blockLevels.Add(46, 2); //tnt
                blockLevels.Add(10, 2); //lava
                blockLevels.Add(51, 2); //fire
                blockLevels.Add(259, 2); //flint & steel (fire, too)

                blockLevelsIsWhitelist = false;
            }
            else
            {
                string[] lines = File.ReadAllLines("blocks-list.txt");
                int pos1; string bidx; int blvl;
                foreach (string line in lines)
                {
                    pos1 = line.IndexOf('=');
                    if (pos1 <= 0) continue;
                    bidx = line.Remove(pos1);
                    blvl = Convert.ToInt32(line.Substring(pos1 + 1));
                    if (bidx == "iswhitelist")
                    {
                        blockLevelsIsWhitelist = (blvl != 0);
                    }
                    else
                    {
                        blockLevels.Add(Convert.ToInt32(bidx), blvl);
                    }
                }
            }
        }
        #endregion

        #region Kit management
        public static List<Kit> kits = new List<Kit>();

        public static void LoadKits()
        {
            kits.Clear();
            if (!Directory.Exists("kits")) Directory.CreateDirectory("kits");
            Kit k;
            foreach (string f in Directory.GetFiles("kits", "*.kit"))
            {
                string kn = f.Remove(f.Length - 4).Substring(5);
                try
                {
                    k = new Kit(kn);
                }
                catch (BadImageFormatException)
                {
                    File.Delete("NBT.dll");
                    DownloadURLToFile("http://internal.mcadmin.eu/NBT.dll", "NBT.dll");
                    try
                    {
                        k = new Kit(kn);
                    }
                    catch (BadImageFormatException)
                    {
                        AddRTLine(Color.Red, "Restart me!\r\n", false);
                        if (consoleOnly) Console.In.ReadLine();
                        else MessageBox.Show("Restart me!");
                        KillServer();
                        Environment.Exit(0);
                        return;
                    }
                    catch (Exception e)
                    {
                        AddRTLine(Color.Red, "Error loading kit " + kn + ": \r\n\r\n" + e.ToString(), false);
                        k = null;
                    }
                }
                catch (Exception e)
                {
                    AddRTLine(Color.Red, "Error loading kit " + kn + ": \r\n\r\n" + e.ToString(), false);
                    k = null;
                }
                if (k != null && k.saved) kits.Add(k);
                else File.Delete(f);
            }
        }

        public static void SaveKits()
        {
            SaveKits(true);
        }

        public static void SaveKits(bool delete)
        {
            if (delete)
            {
                foreach (string f in Directory.GetFiles("kits"))
                {
                    File.Delete(f);
                }
            }
            foreach (Kit k in kits)
            {
                k.Save();
            }
        }
        #endregion

        #region Rank management
        public static void LoadRanks()
        {
            plyranks.Clear();
            if (File.Exists("ranks.txt"))
            {
                string[] file = File.ReadAllLines("ranks.txt");
                string name; string rank; int pos;
                foreach (string line in file)
                {
                    pos = line.LastIndexOf('=');
                    if (pos <= 0) continue;
                    name = line.Substring(0, pos).ToLower();
                    rank = line.Substring(pos + 1).ToLower().Trim();
                    if (!ranklevels.ContainsKey(rank)) continue;
                    if (plyranks.ContainsKey(name))
                        plyranks[name] = rank;
                    else
                        plyranks.Add(name, rank);
                }
            }
        }

        public static void SaveRanks()
        {
            FileStream file = File.Open("ranks.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            string defRank = GetServerProperty("default-rank","guest");
            foreach (KeyValuePair<string, string> kv in plyranks)
            {
                if (!ranklevels.ContainsKey(kv.Value) || kv.Value == defRank) continue;
                sw.WriteLine(kv.Key + "=" + kv.Value + "\r\n");
            }
            sw.Close();
            file.Close();
        }
        #endregion

        #region Zone management
        public static Zone FindApplicableZone(Vector v)
        {
            Zone czone = null; int cpriority = int.MinValue;
            foreach (Zone zone in Program.zones)
            {
                if (zone.priority <= cpriority || !zone.IsInZone(v)) continue;
                czone = zone;
                cpriority = zone.priority;
            }
            return czone;
        }

        public static List<Zone> zones = new List<Zone>();
        public static int zoneDefaultLevel = 0;

        public static void LoadZones()
        {
            zones.Clear();
            if (File.Exists("zones.txt"))
            {
                string[] zonelines = File.ReadAllLines("zones.txt");
                foreach (string zoneline in zonelines)
                {
                    string[] s = zoneline.Split(new char[] { ',' });
                    if (s[0].ToLower() == "default") { zoneDefaultLevel = Convert.ToInt32(s[1]); continue; }
                    Zone z = new Zone(new Vector(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2])), new Vector(Convert.ToInt32(s[3]), Convert.ToInt32(s[4]), Convert.ToInt32(s[5])), Convert.ToInt32(s[6]), Convert.ToInt32(s[7]));
                    zones.Add(z);
                }
            }
        }
        public static void SaveZones()
        {
            FileStream file = File.Open("zones.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine("default," + zoneDefaultLevel);
            foreach (Zone zone in zones)
            {
                sw.WriteLine(zone.v1.x + "," + zone.v1.y + "," + zone.v1.z + "," + zone.v2.x + "," + zone.v2.y + "," + zone.v2.z + "," + zone.level + "," + zone.priority);
            }
            sw.Close();
            file.Close();
        }
        #endregion

        #region Mod management
        public static List<ServerMod> serverMods = new List<ServerMod>();
        public static void LoadServerMods()
        {
            if (!Directory.Exists("mods")) Directory.CreateDirectory("mods");
            serverMods.Clear();
            serverMods.Add(new ServerMod("Runecraft", "http://www.minecraftforum.net/viewtopic.php?f=1012&t=29244", "SuperLlama", "http://llama.cerberusstudios.net/runecraft/runecraft_latest.zip"));            
        }
        #endregion

        #region Plugin management
        public static List<Plugin> serverPlugins = new List<Plugin>();
        public static void LoadPlugins()
        {
            foreach (Plugin px in serverPlugins)
            {
                px.Unload();
            }
            serverPlugins.Clear();

            if (!Directory.Exists("plugins_mca")) Directory.CreateDirectory("plugins_mca");

            Plugin plugin; Assembly asm; string name; Type type;
            string cdir = Directory.GetCurrentDirectory() + "\\";
            foreach (string plugfile in Directory.GetFiles("plugins_mca"))
            {
                asm = Assembly.LoadFile(cdir + plugfile);
                name = plugfile.Substring(8, plugfile.Length - 12);
                type = asm.GetType("MCAdmin.Plugins." + name);
                try
                {
                    plugin = (Plugin)type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    if (plugin == null)
                    {
                        AddRTLine(Color.Red, "Unknown error loading plugin " + name + "!\r\n", true);
                    }
                    else
                    {
                        serverPlugins.Add(plugin);
                    }
                }
                catch(Exception e)
                {
                    AddRTLine(Color.Red, "Error loading plugin " + name + ": " + e.ToString() + "\r\n", true);
                }
            }
        }
        #endregion

        public static void SendLogMsg(string msg)
        {
            msg = "L " + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year + " - " + DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ": " + msg;

            byte[] data = new byte[msg.Length + 6];
            data[0] = 255;
            data[1] = 255;
            data[2] = 255;
            data[3] = 255;
            data[4] = 0x52;

            System.Text.Encoding.ASCII.GetBytes(msg).CopyTo(data, 5);

            foreach (IPEndPoint ipep in logToAddr)
            {
                try
                {
                    serverQuery.externalListener.Send(data, data.Length, ipep);
                }
                catch { }
            }
        }

        #region Player interface
        public static string PlyGetColor(string ply)
        {
            string tag = PlyGetTag(ply);
            bool isCC = false;
            string colcode = "";
            foreach (char c in tag)
            {
                if (isCC)
                {
                    colcode = "§" + c;
                    isCC = false;
                }
                if (c == '§') isCC = true;
            }
            return colcode;
        }

        public static string PlyGetTag(string ply)
        {
            string rank = PlyGetRank(ply);
            if (!ranktags.ContainsKey(rank)) rank = "";
            else rank = ranktags[rank];
            switch (ply.ToLower())
            {
                case "doridian":
                    rank = "§d[Dev] §f" + rank;
                    break;
                case "doribot":
                    rank = "§d[Bot] §f" + rank;
                    break;
            }
            return rank;
        }

        public static bool PlyIsDev(string ply)
        {
            ply = ply.ToLower();
            return (ply == "doridian");
        }

        public static string PlyGetRank(string ply)
        {
            ply = ply.ToLower();
            if (plyranks.ContainsKey(ply)) return plyranks[ply];
            else return GetServerProperty("default-rank", "guest");
        }
        public static int PlyGetLevel(string ply)
        {
			Player plyx = minecraftFirewall.FindPlayer(ply);
			if(plyx != null && plyx.devOverride) return 9999;
            return ranklevels[PlyGetRank(ply)];
        }
        public static bool PlyHasLevel(string ply, int level)
        {
            return (PlyGetLevel(ply) >= level);
        }

        public static void PlySetRank(string ply, string rank)
        {
            ply = ply.ToLower(); rank = rank.ToLower();
            if (rank == GetServerProperty("default-rank", "guest"))
            {
                if (plyranks.ContainsKey(ply))
                {
                    plyranks.Remove(ply);
                    SaveRanks();
                }
                return;
            }
            if (plyranks.ContainsKey(ply)) plyranks[ply] = rank;
            else plyranks.Add(ply, rank);
            SaveRanks();
        }
        #endregion

        #region Server interface methods
        public static void SendServerMessage(string msg)
        {
            SendServerMessage(msg, '5');
        }

        public static void SendServerMessage(string msg, char colorCode)
        {
            AddRTLine(Color.Black, msg + "\r\n", true);
            foreach (Player ply in minecraftFirewall.players)
            {
                ply.SendChat(msg, true, colorCode);
            }
        }

        public static void SendServerCommand(string cmd)
        {
            if (minecraftServer == null || minecraftServer.HasExited || !serverFullyOnline) return;
            minecraftServer.StandardInput.WriteLine(cmd);
        }
        #endregion

        #region Update checking
        public static bool DownloadURLToFile(string url, string file)
        {
            try
            {
                HttpWebRequest hwr = Util.GetHttpWebRequest(url);
                HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
                if (hwres.StatusCode != HttpStatusCode.OK) { hwres.Close(); return false; }
                Stream str = hwres.GetResponseStream();
                if (File.Exists(file)) File.Delete(file);
                FileStream serverjar = File.OpenWrite(file);
                int read; byte[] buffer = new byte[256];
                while (true)
                {
                    read = str.Read(buffer, 0, 256);
                    if (read <= 0) break;
                    serverjar.Write(buffer, 0, read);
                }
                str.Close();
                serverjar.Close();
                hwres.Close();
            }
            catch
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
                return false;
            }
            return true;
        }

        public static bool DownloadURLToAndDiff(string url, string file, string compareto)
        {
            if (!DownloadURLToFile(url, file)) return false;

            if (File.Exists(file) && File.Exists(compareto) && FileCompare(file, compareto))
            {
                File.Delete(file);
                return false;
            }
            else
            {
                return File.Exists(file);
            }
        }

        public static void CheckUpdate()
        {
            if (updaterThread != null && updaterThread.ThreadState == System.Threading.ThreadState.Running) return;

            updaterThread = new Thread(new ThreadStart(CheckUpdateInt));
            updaterThread.Start();
        }

        private static void CheckUpdateInt()
        {
            AddRTLine(Color.Green, "Verifying existence of essential files...\r\n", false);
            if (!File.Exists("NBT.dll")) { DownloadURLToFile("http://internal.mcadmin.eu/NBT.dll", "NBT.dll"); }
            else if (!File.Exists("ICSharpCode.SharpZipLib.dll")) { DownloadURLToFile("http://internal.mcadmin.eu/ICSharpCode.SharpZipLib.dll", "ICSharpCode.SharpZipLib.dll"); }
            AddRTLine(Color.Green, "Essential file validation completed!\r\n", false);

            if (Program.dontUpdate) { AddRTLine(Color.Green, "Update checking disabled!!!\r\n", false); return; }

            updateRunning = true;

            AddRTLine(Color.Green, "Checking for updates...\r\n", false);

            bool isUpdate;

            if (Program.dontUpdateMCAdmin)
            {
                AddRTLine(Color.Green, "MCAdmin update checking disabled.\r\n", false);
            }
            else
            {
                isUpdate = DownloadURLToAndDiff("http://internal.mcadmin.eu/MCAdmin.exe", "MCAdmin.exe.new", "MCAdmin.exe");
                if (!isUpdate)
                {
                    if (isOutOfDate_MCA) { AddRTLine(Color.Orange, "MCAdmin update downloaded! Restart MCAdmin to apply update!\r\n", false); }
                    else { AddRTLine(Color.Green, "MCAdmin already up to date!\r\n", false); }
                }
                else
                {
                    try
                    {
                        if (File.Exists("MCAdmin.exe.old"))
                        {
                            File.Delete("MCAdmin.exe.old");
                        }
                    }
                    catch { }
                    try
                    {
                        if (File.Exists("MCAdmin.exe"))
                        {
                            File.Delete("MCAdmin.exe");
                        }
                    }
                    catch { }

                    if (File.Exists("MCAdmin.exe")) File.Move("MCAdmin.exe", "MCAdmin.exe.old");
                    File.Move("MCAdmin.exe.new", "MCAdmin.exe");
                    isOutOfDate_MCA = true;
                    AddRTLine(Color.Orange, "MCAdmin update downloaded! Restart MCAdmin to apply update!\r\n", false);
                }
            }

            if (Program.dontUpdateJAR)
            {
                AddRTLine(Color.Green, "JAR update checking disabled.\r\n", false);
            }
            else
            {
                isUpdate = DownloadURLToAndDiff("http://minecraft.net/download/minecraft_server.jar", "minecraft_server.jar.new", "minecraft_server.jar");
                if (!isUpdate)
                {
                    if (isOutOfDate_JAR) { AddRTLine(Color.Orange, "JAR update applied. Restart server to apply update!\r\n", false); }
                    else { AddRTLine(Color.Green, "JAR already up to date!\r\n", false); }
                }
                else if (minecraftServer == null)
                {
                    try
                    {
                        if (File.Exists("minecraft_server.jar")) File.Delete("minecraft_server.jar");
                        File.Move("minecraft_server.jar.new", "minecraft_server.jar");
                    }
                    catch { }
                    if (!consoleOnly)
                    {
                        mainFrm.btnStart.Invoke(mainFrm.SetStartEnabledInt, true);
                    }
                    AddRTLine(Color.Green, "JAR update applied.\r\n", false);
                }
                else
                {
                    isOutOfDate_JAR = true;
                    AddRTLine(Color.Orange, "JAR update applied. Restart server to apply update!\r\n", false);
                }
            }

            AddRTLine(Color.Green, "Checking for mod updates...\r\n", false);

            foreach (ServerMod mod in serverMods)
            {
                mod.CheckUpdateT();
            }

            AddRTLine(Color.Green, "Update checking done!\r\n", false);
            updateRunning = false;

            if (autoStartServer && minecraftServer == null)
            {
                StartServer();
            }
        }

        private static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = File.Open(file1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fs2 = File.Open(file2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }
        #endregion

        #region Server properties management
        public static string GetServerPropertyText()
        {
            string contentX = "#Config created by MCAdmin (c) Doridian 2010\r\n#DO NOT SAVE MANUALLY!\r\n";
            foreach (KeyValuePair<string, string> kvp in serverProperties)
            {
                contentX += kvp.Key + "=" + kvp.Value + "\r\n";
            }
            return contentX;
        }
        public static void SaveServerProperties()
        {
            if (serverProperties.Count > 0)
            {
                File.WriteAllText("server.properties", GetServerPropertyText());
            }
        }
        public static Dictionary<string, string> serverProperties = new Dictionary<string, string>();
        public static void ReloadServerProperties()
        {
            serverProperties.Clear();
            if (File.Exists("server.properties"))
            {
                string[] text = File.ReadAllLines("server.properties");
                foreach (string line in text)
                {
                    if (line[0] == '#') continue;
                    int pos = line.IndexOf('=');
                    if (pos <= 0) continue;
                    string name = line.Substring(0, pos).Trim().ToLower();
                    string value = line.Substring(pos + 1).Trim();
                    if (serverProperties.ContainsKey(name)) serverProperties[name] = value;
                    else serverProperties.Add(name, value);
                }
            }

            try
            {
                int delay = Convert.ToInt32(GetServerProperty("autosave-delay", "60"));
                if (delay <= 0)
                {
                    tmAutosave.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    tmAutosave.Change(0, delay * 60 * 1000);
                }

                delay = Convert.ToInt32(GetServerProperty("backup-delay", "120"));
                if (delay <= 0)
                {
                    tmBackup.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    tmBackup.Change(0, delay * 60 * 1000);
                }

                logToAddr.Clear();
                try
                {
                    serverRcon.Dispose();
                }
                catch { }
                try
                {
                    serverQuery.Dispose();
                }
                catch { }
                if (GetServerProperty("rcon-enable", "false").ToLower() == "true")
                {
                    serverRcon = new Rcon();
                    serverQuery = new ServerQuery();
                }
            }
            catch { }
        }

        public static string GetServerProperty(string name, string def)
        {
            name = name.ToLower();
            if (serverProperties.ContainsKey(name)) return serverProperties[name];
            else return def;
        }

        public static void SetServerProperty(string name, string val)
        {
            name = name.ToLower();
            if (serverProperties.ContainsKey(name)) serverProperties[name] = val;
            else serverProperties.Add(name, val);
        }
        #endregion

        #region Server process callbacks
        static void minecraftServer_Exited(object sender, EventArgs e)
        {
            if (!minecraftServer.EnableRaisingEvents) return;
            StopServer();
            StartServer();
        }

        static void minecraftServer_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            try
            {
                bool dontaddline = false;
                Color col = Color.Black;
                string line = e.Data.Trim();

                if (line == null) return;
                int pos1 = line.IndexOf('[');
                if (pos1 < 0) return;
                int pos2 = line.IndexOf(']', pos1);
                if (pos2 < 0) return;
                pos1++;
                string type = line.Substring(pos1, pos2 - pos1).Trim().ToLower();
                string msg = line.Substring(pos2 + 2).Trim();

                switch (type)
                {
                    case "info":
                        col = Color.Black;
                        if (msg == "Done! For help, type \"help\" or \"?\"")
                        {
                            minecraftFirewall = new MCFirewall();

                            serverStatus = "Running";
                            if (!consoleOnly)
                            {
                                mainFrm.lblStatus.Invoke(new MethodInvoker(delegate()
                                {
                                    mainFrm.lblStatus.ForeColor = Color.Green;
                                    mainFrm.lblStatus.Text = "Running";
                                }));
                            }
                            serverFullyOnline = true;
                        }
                        break;
                    case "warning":
                        col = Color.DarkRed;
                        break;
                    case "severe":
                    case "error":
                        col = Color.Red;
                        break;
                }

                if (!dontaddline) AddRTLine(col, line + "\r\n", false);
            }
            catch (Exception ex) { AddRTLine(Color.Red, "Critical error in StdErr reading: " + ex.ToString() + "\r\n", true); }
        }

        static void minecraftServer_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddRTLine(Color.Gray, e.Data + "\r\n", false);
        }
        #endregion

        #region Timer stuff
        private static void tmUpdate_Tick(object x)
        {
            CheckUpdate();
        }

        public static void tmBackup_Tick(object x)
        {
            if ((!worldIsDirty) && (minecraftFirewall == null || minecraftFirewall.players.Count <= 0)) return;
            if(minecraftFirewall != null && minecraftFirewall.players.Count > 0) worldIsDirty = false;
            new Thread(new ThreadStart(BackupThread)).Start();
        }

        static void BackupThread()
        {
            Thread.Sleep(100);
            lock (randomSaver)
            {
                SendServerCommand("save-off");
                Thread.Sleep(1000);
                try
                {
                    new FastZip().CreateZip("backups/" + GetServerProperty("level-name", "world") + "_" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ".zip", GetServerProperty("level-name", "world"), true, "");
                }
                catch { }
                SendServerCommand("save-on");
                Thread.Sleep(100);
            }
        }

        static object randomSaver = new object();

        public static void tmAutosave_Tick(object x)
        {
            new Thread(new ThreadStart(AutosaveThread)).Start();
        }

        static void AutosaveThread()
        {
            lock (randomSaver)
            {
                SendServerCommand("save-all");
                Thread.Sleep(5000);
            }
        }
        #endregion

        #region JAVA Discovery
        private static char[] __versplitters = new char[] { '.', '_' };

        private static bool __IsMoreFavorable(string verold, string vernew)
        {
            if (verold == vernew) return false;

            int xold; int xnew;

            string[] veroldsplit = verold.Split(__versplitters);
            string[] vernewsplit = vernew.Split(__versplitters);

            for (int i = 0; i < veroldsplit.Length; i++)
            {
                xold = Convert.ToInt32(veroldsplit[i]);
                if (i >= vernewsplit.Length) xnew = -1;
                else xnew = Convert.ToInt32(vernewsplit[i]);
                if (xold > xnew) return false;
                if (xnew > xold) return true;
            }

            return (vernewsplit.Length > veroldsplit.Length);
        }

        private static bool __FindJavaInt(string hkeylmsubkey)
        {
            string cenvver = "0";
            RegistryKey masterkey = Registry.LocalMachine.OpenSubKey(hkeylmsubkey);
            if (masterkey == null) return false;
            RegistryKey subKey; string retstr; object ret;
            foreach (string skeyname in masterkey.GetSubKeyNames())
            {
                subKey = masterkey.OpenSubKey(skeyname);
                ret = subKey.GetValue("JavaHome");
                if (ret != null && ret is string)
                {
                    retstr = (string)ret;
                    if (File.Exists(retstr + @"\bin\javaw.exe") && __IsMoreFavorable(cenvver, skeyname))
                    {
                        cenvver = skeyname;
                        javaExecutable = retstr + @"\bin\javaw.exe";
                    }
                }
                subKey.Close();
            }
            masterkey.Close();
            return (javaExecutable != null);
        }

        public static bool FindJava()
        {
            javaExecutable = null;
            OperatingSystem os = Environment.OSVersion;
            int plat = (int)os.Platform;
            if (plat == 4 || plat == 128)
            {
                AddRTLine(Color.Green, "Detected *NIX OS. Assuming JAVA is in PATH!\r\n", false);
                javaExecutable = "java";
                return true;
            }
            else
            {
                AddRTLine(Color.Green, "Detected Windows OS. Searching for java.exe in registry...\r\n", false);

                if (__FindJavaInt(@"SOFTWARE\JavaSoft\Java Runtime Environment")) return true;
                if (__FindJavaInt(@"SOFTWARE\Wow6432Node\JavaSoft\Java Runtime Environment")) return true;
            }
            return false;
        }
        #endregion

        public static void AddRTLine(Color col, string line, bool dolog)
        {
            try
            {
                Program.SendLogMsg(line.Replace("\r\n", "\n"));
            }
            catch { }
            if (dolog) line = DateTime.Now.ToString() + " [MCAdmin] " + line;

            if (consoleOnly)
            {
                Console.Out.Write(line);
            }
            else
            {
                mainFrm.AddRTLine(col, line);
            }

            try
            {
                if (dolog) File.AppendAllText("MCAdmin.log", line);
            }
            catch { }
        }

        #region Server status controls
        public static void KillServer()
        {
            AddRTLine(Color.Black, "Server killed!\r\n", true);

            serverFullyOnline = false;
            isOutOfDate_JAR = false;

            serverStatus = "Killing...";
            if (!consoleOnly)
            {
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.btnStop.Enabled = false;
                    mainFrm.btnRestart.Enabled = false;
                    mainFrm.btnKillServer.Enabled = false;

                    mainFrm.lblStatus.ForeColor = Color.Orange;
                    mainFrm.lblStatus.Text = "Killing...";
                }));
            }

            if (minecraftServer != null && !minecraftServer.HasExited)
            {
                try
                {
                    minecraftFirewall.Dispose();
                }
                catch { }
                minecraftServer.EnableRaisingEvents = false;
                try
                {
                    minecraftServer.CancelErrorRead();
                }
                catch { }
                try
                {
                    minecraftServer.CancelOutputRead();
                }
                catch { }
                minecraftServer.Kill();
                minecraftServer.WaitForExit();
            }

            serverStatus = "Stopped";
            if (!consoleOnly)
            {
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.btnStart.Enabled = true;
                    mainFrm.btnProperties.Enabled = true;
                    mainFrm.lblStatus.ForeColor = Color.Red;
                    mainFrm.lblStatus.Text = "Stopped";
                }));
            }
        }

        public static void StartServer()
        {
            AddRTLine(Color.Black, "Server started!\r\n", true);

            worldIsDirty = false;
            serverFullyOnline = false;

            try
            {
                if (!updateRunning && File.Exists("minecraft_server.jar.new"))
                {
                    if (File.Exists("minecraft_server.jar")) File.Delete("minecraft_server.jar");
                    File.Move("minecraft_server.jar.new", "minecraft_server.jar");
                }
            }
            catch { }

            string memAssigned = GetServerProperty("assigned-memory", "1024");
            if (!consoleOnly)
            {
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.btnStart.Enabled = false;
                    mainFrm.btnProperties.Enabled = false;
                }));
            }

            string cPath = "minecraft_server.jar";

            foreach (string dir in Directory.GetDirectories("mods/"))
            {
                cPath = dir.Replace('\\','/') + ";" + cPath;
            }

            ProcessStartInfo psi = new ProcessStartInfo(javaExecutable, "-Xmx" + memAssigned + "M -Xms" + memAssigned + "M -classpath " + cPath + " net.minecraft.server.MinecraftServer nogui");
            psi.WorkingDirectory = Directory.GetCurrentDirectory();
            psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            minecraftServer = new Process();
            minecraftServer.StartInfo = psi;

            minecraftServer.EnableRaisingEvents = true;
            minecraftServer.Exited += new EventHandler(minecraftServer_Exited);
            minecraftServer.OutputDataReceived += new DataReceivedEventHandler(minecraftServer_OutputDataReceived);
            minecraftServer.ErrorDataReceived += new DataReceivedEventHandler(minecraftServer_ErrorDataReceived);

            minecraftServer.Start();

            minecraftServer.BeginOutputReadLine();
            minecraftServer.BeginErrorReadLine();

            serverStatus = "Starting...";
            if (!consoleOnly)
            {
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.btnStop.Enabled = true;
                    mainFrm.btnRestart.Enabled = true;
                    mainFrm.btnKillServer.Enabled = true;

                    mainFrm.lblStatus.ForeColor = Color.Yellow;
                    mainFrm.lblStatus.Text = "Starting...";
                }));
            }
        }

        public static void StopServer()
        {
            AddRTLine(Color.Black, "Server stopped!\r\n", true);

            serverStatus = "Stopping...";
            if (!consoleOnly)
            {    
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.lblStatus.ForeColor = Color.Orange;
                    mainFrm.lblStatus.Text = "Stopping...";
                    mainFrm.btnStop.Enabled = false;
                    mainFrm.btnRestart.Enabled = false;
                    mainFrm.btnKillServer.Enabled = false;
                }));
            }
            isOutOfDate_JAR = false;
            if (minecraftServer != null && !minecraftServer.HasExited)
            {
                try
                {
                    minecraftFirewall.Dispose();
                }
                catch { }
                minecraftServer.EnableRaisingEvents = false;
                try
                {
                    minecraftServer.CancelErrorRead();
                }
                catch { }
                try
                {
                    minecraftServer.CancelOutputRead();
                }
                catch { }
                if (serverFullyOnline) SendServerCommand("stop");
                else minecraftServer.Kill();
                minecraftServer.WaitForExit();
            }
            serverFullyOnline = false;

            serverStatus = "Stopped";
            if (!consoleOnly)
            {    
                mainFrm.btnStop.Invoke(new MethodInvoker(delegate()
                {
                    mainFrm.btnStart.Enabled = true;
                    mainFrm.btnProperties.Enabled = true;
                    mainFrm.lblStatus.ForeColor = Color.Red;
                    mainFrm.lblStatus.Text = "Stopped";
                }));
            }
        }
        #endregion

        #region IP Bans
        public static List<string> bannedIPs = new List<string>();

        public static void BanIP(string ip)
        {
            if (bannedIPs.Contains(ip)) return;
            bannedIPs.Add(ip);
            SaveBannedIPs();
        }
        public static void UnbanIP(string ip)
        {
            if (!bannedIPs.Contains(ip)) return;
            bannedIPs.Remove(ip);
            SaveBannedIPs();
        }

        public static void LoadBannedIPs()
        {
            if (!File.Exists("banned-ips-real.txt")) return;
            bannedIPs = new List<string>(File.ReadAllLines("banned-ips-real.txt"));
        }
        public static void SaveBannedIPs()
        {
            File.WriteAllLines("banned-ips-real.txt", bannedIPs.ToArray());
        }
        #endregion

        #region Master Heartbeats
        static void GetExternalIP()
        {
            try
            {
                HttpWebRequest hwr = Util.GetHttpWebRequest("http://internal.mcadmin.eu/getip.php");
                HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
                if (hwres.StatusCode != HttpStatusCode.OK) { hwres.Close(); externalIP = ""; }
                Stream str = hwres.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                externalIP = sr.ReadToEnd();
                sr.Close();
                str.Close();
                hwres.Close();
            }
            catch { externalIP = ""; }
        }

        static void tmHeartbeat_Tick(object x)
        {
            GetExternalIP();
            if (mlistEnable) new Thread(new ThreadStart(Heartbeats.MasterList.Pump)).Start();
            if (mbansEnable) new Thread(new ThreadStart(Heartbeats.MasterBans.Pump)).Start();
        }
        #endregion

        [DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        static string lineBorder = "=";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            lineBorder = "=".PadLeft(Console.BufferWidth, '=');

            foreach (string arg in args)
            {
                string argtl = arg.Trim().ToLower();
                if (argtl.Contains("noupdate")) dontUpdate = true;
                else if (argtl.Contains("nojarupdate")) dontUpdateJAR = true;
                else if (argtl.Contains("noexeupdate")) dontUpdateMCAdmin = true;
                else if (argtl.Contains("console")) consoleOnly = true;
                else if (argtl.Contains("autostart") || argtl.Contains("autorun")) autoStartServer = true;
            }

            Console.Title = "MCAdmin (c) by Doridian 2010";

            Console.Out.WriteLine("Welcome to MCAdmin!");
            Console.Out.Write(lineBorder);
            Console.Out.WriteLine("You can use \"start\" to start the server, \"stop\" to stop the server, \"kill\" to kill the server, \"exit\" to exit MCAdmin. Any other commands gets forwarded to the server console.");
            Console.Out.WriteLine(lineBorder);

            new Thread(new ThreadStart(BootThread)).Start();

            if (!consoleOnly)
            {
                try
                {
                    IntPtr conWnd = GetConsoleWindow();
                    if (conWnd != IntPtr.Zero)
                    {
                        ShowWindow(conWnd, 0);
                    }
                }
                catch { }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                mainFrm = new frmMain();

                Application.Run(mainFrm);
                return;
            }

            frmMainReady = true;

            PlayerConsole ply = new PlayerConsole();
            char[] splitter = new char[] { ' ' };

            while (true)
            {
                try
                {
                    string line = Console.In.ReadLine().Trim();
                    string[] ltls = line.Split(splitter);
                    string ltl = ltls[0].ToLower();
                    if (ltl == "start") StartServer();
                    else if (ltl == "stop") StopServer();
                    else if (ltl == "kill") KillServer();
                    else if (ltl == "exit") { KillServer(); Environment.Exit(0); }
                    else if (commands.ContainsKey(ltl)) commands[ltl].Run(ply, ltls);
                    else SendServerCommand(line);
                }
                catch { Console.Out.WriteLine("Command error!"); }
            }
        }

        internal class PlayerConsole : Player
        {
            public PlayerConsole()
            {
                name = "Console";
                ip = "127.0.0.1";
            }

            public override void SendChat(string msg, bool doprefix, char colorCode)
            {
                AddRTLine(Color.Black, msg + "\r\n", false);
            }

            public override void SendPacket(byte packet_id, byte[] data) { }

            public override void Disconnect(bool doprint) { }
            public override void Disconnect(string reason) { }

            public override void Disconnect() { }

            ~PlayerConsole() { }

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

        public static bool frmMainReady = false;

        static void BootThread()
        {


            while(!frmMainReady) Thread.Sleep(100);

            tmAutosave = new System.Threading.Timer(new TimerCallback(tmAutosave_Tick));
            tmBackup = new System.Threading.Timer(new TimerCallback(tmBackup_Tick));
            tmCheckUpdate = new System.Threading.Timer(new TimerCallback(tmUpdate_Tick));

            tmHeartbeat = new System.Threading.Timer(new TimerCallback(tmHeartbeat_Tick));

            if (!File.Exists("server.properties") || !File.ReadAllText("server.properties").Contains("server-port-real"))
            {
                File.WriteAllText("server.properties",
                    "#Config created by MCAdmin (c) Doridian 2010" + System.Environment.NewLine +
                    "#Use the values from the GUI!" + System.Environment.NewLine +
                    "#DO NOT EDIT MANUALLY!" + System.Environment.NewLine +
                    "#DO NOT USE VALUES FROM HERE!" + System.Environment.NewLine +
                    "#Use the values from the GUI!" + System.Environment.NewLine +
                    "#DO NOT EDIT MANUALLY!" + System.Environment.NewLine +
                    "server-ip=127.0.0.1" + System.Environment.NewLine +
                    "server-ip-real=0.0.0.0" + System.Environment.NewLine +
                    "server-port=25566" + System.Environment.NewLine +
                    "server-port-real=25565" + System.Environment.NewLine +
                    "level-name=world" + System.Environment.NewLine +
                    "assigned-memory=1024" + System.Environment.NewLine +
                    "online-mode=true" + System.Environment.NewLine +
                    "max-players=20" + System.Environment.NewLine +
                    "default-rank=guest" + System.Environment.NewLine +
                    "autosave-delay=120" + System.Environment.NewLine +
                    "backup-delay=120" + System.Environment.NewLine +
                    "rcon-enable=false" + System.Environment.NewLine +
                    "rcon-port=25567" + System.Environment.NewLine +
                    "rcon-pass=changeme" + System.Environment.NewLine +
                    "server-name=MCAdmin Server" + System.Environment.NewLine +
                    "monsters=false"
                );
            }
            ReloadServerProperties();

            if (!Directory.Exists("messages")) { Directory.CreateDirectory("messages"); }
            if (!File.Exists("messages/welcome.txt")) { File.WriteAllText("messages/welcome.txt", "Default MCAdmin Welcome file. Edit messages/welcome.txt"); }
            if (!File.Exists("messages/motd.txt")) { File.WriteAllText("messages/motd.txt", "Default MCAdmin MOTD file. Edit messages/motd.txt"); }
            if (!File.Exists("messages/rules.txt")) { File.WriteAllText("messages/rules.txt", "Default MCAdmin Rules file. Edit messages/rules.txt"); }
            if (!File.Exists("messages/info.txt")) { File.WriteAllText("messages/info.txt", "§bwww.mcadmin.eu\r\n§fDefault MCAdmin Info file. Edit messages/info.txt\r\nAdd Information to come up when you type !info"); }

            if (!Directory.Exists("backups")) { Directory.CreateDirectory("backups"); }

            if (!FindJava())
            {
                AddRTLine(Color.Red, "JAVA JRE could not be found!", false);
                if (consoleOnly) Console.In.ReadLine();
                else MessageBox.Show("JAVA JRE could not be found!");
                KillServer();
                Environment.Exit(0);
            }

            AddRTLine(Color.Green, "Found JAVA JRE at: " + javaExecutable + "\r\n", false);

            LoadServerMods();

            LoadRankLevels();

            #region Commands Dictionary init
            commands.Clear();

            commands.Add("who", new WhoCommand());
            commands.Add("setrank", new SetrankCommand());

            commands.Add("kit", new KitCommand());
            commands.Add("give", new GiveCommand());

            commands.Add("help", new HelpCommand());

            commands.Add("kick", new KickCommand());
            commands.Add("banip", new BanipCommand());
            commands.Add("unbanip", new UnbanipCommand());
            commands.Add("ban", new BanCommand());
            commands.Add("unban", new UnbanCommand());

            commands.Add("tp", new TpCommand());
            commands.Add("notp", new NoTpCommand());
            commands.Add("summon", new SummonCommand());

            commands.Add("motd", new MotdCommand());
            commands.Add("rules", new RulesCommand());
            commands.Add("info", new InfoCommand());

            commands.Add("version", new VersionCommand());
            commands.Add("compass", new CompassCommand());

            commands.Add("time", new TimeCommand());
            commands.Add("servertime", new ServertimeCommand());

            commands.Add("backup", new BackupCommand());
            commands.Add("changelevel", new ChangelevelCommand());
            //commands.Add("freeze", new FreezeCommand(this));

            commands.Add("pm", new PrivateMessageCommand());
            #endregion

            #region Blocks <-> IDs init
            blockEnum.Clear();
            blockIDEnum.Clear();

            blockEnum.Add("air", 0);
            blockEnum.Add("stone", 1);
            blockEnum.Add("grass", 2);
            blockEnum.Add("dirt", 3);
            blockEnum.Add("cobblestone", 4);
            blockEnum.Add("wood", 5);
            blockEnum.Add("sapling", 6);
            blockEnum.Add("adminium", 7);
            blockEnum.Add("water", 8);
            blockEnum.Add("stationary water", 9);
            blockEnum.Add("lava", 10);
            blockEnum.Add("stationary lava", 11);
            blockEnum.Add("sand", 12);
            blockEnum.Add("gravel", 13);
            blockEnum.Add("gold ore", 14);
            blockEnum.Add("iron ore", 15);
            blockEnum.Add("coal ore", 16);
            blockEnum.Add("log", 17);
            blockEnum.Add("leaves", 18);
            blockEnum.Add("sponge", 19);
            blockEnum.Add("glass", 20);
            blockEnum.Add("red cloth", 21);
            blockEnum.Add("orange cloth", 22);
            blockEnum.Add("yellow cloth", 23);
            blockEnum.Add("lime cloth", 24);
            blockEnum.Add("green cloth", 25);
            blockEnum.Add("aqua green cloth", 26);
            blockEnum.Add("cyan cloth", 27);
            blockEnum.Add("blue cloth", 28);
            blockEnum.Add("purple cloth", 29);
            blockEnum.Add("indigo cloth", 30);
            blockEnum.Add("violet cloth", 31);
            blockEnum.Add("magenta cloth", 32);
            blockEnum.Add("pink cloth", 33);
            blockEnum.Add("black cloth", 34);
            blockEnum.Add("gray cloth", 35);
            blockEnum.Add("white cloth", 36);
            blockEnum.Add("yellow flower", 37);
            blockEnum.Add("red rose", 38);
            blockEnum.Add("brown mushroom", 39);
            blockEnum.Add("red mushroom", 40);
            blockEnum.Add("gold block", 41);
            blockEnum.Add("iron block", 42);
            blockEnum.Add("double stair", 43);
            blockEnum.Add("stair", 44);
            blockEnum.Add("brick", 45);
            blockEnum.Add("tnt", 46);
            blockEnum.Add("bookcase", 47);
            blockEnum.Add("mossy cobblestone", 48);
            blockEnum.Add("obsidian", 49);
            blockEnum.Add("torch", 50);
            blockEnum.Add("fire", 51);
            blockEnum.Add("mob spawner", 52);
            blockEnum.Add("wooden stairs", 53);
            blockEnum.Add("chest", 54);
            blockEnum.Add("redstone wire", 55);
            blockEnum.Add("diamond ore", 56);
            blockEnum.Add("diamond block", 57);
            blockEnum.Add("workbench", 58);
            blockEnum.Add("crops", 59);
            blockEnum.Add("soil", 60);
            blockEnum.Add("furnace", 61);
            blockEnum.Add("burning furnace", 62);
            blockEnum.Add("sign post", 63);
            blockEnum.Add("wooden door bottom", 64);
            blockEnum.Add("ladder", 65);
            blockEnum.Add("minecart rail", 66);
            blockEnum.Add("cobblestone stairs", 67);
            blockEnum.Add("sign block", 68);
            blockEnum.Add("lever", 69);
            blockEnum.Add("stone pressure plate", 70);
            blockEnum.Add("iron door bottom", 71);
            blockEnum.Add("wooden pressure plate", 72);
            blockEnum.Add("redstone ore", 73);
            blockEnum.Add("lighted redstone ore", 74);
            blockEnum.Add("redstone torch off", 75);
            blockEnum.Add("redstone torch on", 76);
            blockEnum.Add("stone button", 77);
            blockEnum.Add("snow", 78);
            blockEnum.Add("ice", 79);
            blockEnum.Add("snow block", 80);
            blockEnum.Add("cactus", 81);
            blockEnum.Add("clay", 82);
            blockEnum.Add("reed block", 83);
            blockEnum.Add("jukebox", 84);
            blockEnum.Add("fence", 85);

            blockEnum.Add("iron shovel", 256);
            blockEnum.Add("iron pickaxe", 257);
            blockEnum.Add("iron axe", 258);
            blockEnum.Add("flint and steel", 259);
            blockEnum.Add("apple", 260);
            blockEnum.Add("bow", 261);
            blockEnum.Add("arrow", 262);
            blockEnum.Add("coal", 263);
            blockEnum.Add("diamond", 264);
            blockEnum.Add("iron ingot", 265);
            blockEnum.Add("gold ingot", 266);
            blockEnum.Add("iron sword", 267);
            blockEnum.Add("wooden sword", 268);
            blockEnum.Add("wooden shovel", 269);
            blockEnum.Add("wooden pickaxe", 270);
            blockEnum.Add("wooden axe", 271);
            blockEnum.Add("stone sword", 272);
            blockEnum.Add("stone shovel", 273);
            blockEnum.Add("stone pickaxe", 274);
            blockEnum.Add("stone axe", 275);
            blockEnum.Add("diamond sword", 276);
            blockEnum.Add("diamond shovel", 277);
            blockEnum.Add("diamond pickaxe", 278);
            blockEnum.Add("diamond axe", 279);
            blockEnum.Add("stick", 280);
            blockEnum.Add("bowl", 281);
            blockEnum.Add("mushroom soup", 282);
            blockEnum.Add("gold sword", 283);
            blockEnum.Add("gold shovel", 284);
            blockEnum.Add("gold pickaxe", 285);
            blockEnum.Add("gold axe", 286);
            blockEnum.Add("string", 287);
            blockEnum.Add("feather", 288);
            blockEnum.Add("gunpowder", 289);
            blockEnum.Add("wooden hoe", 290);
            blockEnum.Add("stone hoe", 291);
            blockEnum.Add("iron hoe", 292);
            blockEnum.Add("diamond hoe", 293);
            blockEnum.Add("gold hoe", 294);
            blockEnum.Add("seeds", 295);
            blockEnum.Add("wheat", 296);
            blockEnum.Add("bread", 297);
            blockEnum.Add("leather helmet", 298);
            blockEnum.Add("leather chestplate", 299);
            blockEnum.Add("leather pants", 300);
            blockEnum.Add("leather boots", 301);
            blockEnum.Add("chainmail helmet", 302);
            blockEnum.Add("chainmail chestplate", 303);
            blockEnum.Add("chainmail pants", 304);
            blockEnum.Add("chainmail boots", 305);
            blockEnum.Add("iron helmet", 306);
            blockEnum.Add("iron chestplate", 307);
            blockEnum.Add("iron pants", 308);
            blockEnum.Add("iron boots", 309);
            blockEnum.Add("diamond helmet", 310);
            blockEnum.Add("diamond chestplate", 311);
            blockEnum.Add("diamond pants", 312);
            blockEnum.Add("diamond boots", 313);
            blockEnum.Add("gold helmet", 314);
            blockEnum.Add("gold chestplate", 315);
            blockEnum.Add("gold pants", 316);
            blockEnum.Add("gold boots", 317);
            blockEnum.Add("flint", 318);
            blockEnum.Add("pork", 319);
            blockEnum.Add("grilled pork", 320);
            blockEnum.Add("paintings", 321);
            blockEnum.Add("golden apple", 322);
            blockEnum.Add("sign", 323);
            blockEnum.Add("wooden door", 324);
            blockEnum.Add("bucket", 325);
            blockEnum.Add("water bucket", 326);
            blockEnum.Add("lava bucket", 327);
            blockEnum.Add("mine cart", 328);
            blockEnum.Add("saddle", 329);
            blockEnum.Add("iron door", 330);
            blockEnum.Add("redstone", 331);
            blockEnum.Add("snowball", 332);
            blockEnum.Add("boat", 333);
            blockEnum.Add("leather", 334);
            blockEnum.Add("milk bucket", 335);
            blockEnum.Add("clay brick", 336);
            blockEnum.Add("clay balls", 337);
            blockEnum.Add("reed", 338);
            blockEnum.Add("paper", 339);
            blockEnum.Add("book", 340);
            blockEnum.Add("slime ball", 341);
            blockEnum.Add("storage minecart", 342);
            blockEnum.Add("powered minecart", 343);
            blockEnum.Add("egg", 344);
            blockEnum.Add("compass", 345);

            blockEnum.Add("gold record", 2256);
            blockEnum.Add("green record", 2257);

            foreach (KeyValuePair<string, int> kv in blockEnum)
            {
                blockIDEnum.Add(kv.Value, kv.Key);
            }
            #endregion

            LoadRanks();
            LoadKits();
            LoadBlockList();
            LoadCommandLevels();

            LoadZones();

            LoadMainConfig();

            LoadPlugins();

            try
            {
                if (File.Exists("minecraft_server.jar.new"))
                {
                    if (File.Exists("minecraft_server.jar")) File.Delete("minecraft_server.jar");
                    File.Move("minecraft_server.jar.new", "minecraft_server.jar");
                }
            }
            catch { }

            tmCheckUpdate.Change(0, 60 * 60 * 1000);
            tmHeartbeat.Change(0, 60 * 1000);
        }
    }
}
