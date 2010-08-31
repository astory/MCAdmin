using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using MCAdmin.Commands;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Net.Sockets;

namespace MCAdmin
{
    public partial class frmMain : Form
    {
        #region Header variables
        public List<IPEndPoint> logToAddr = new List<IPEndPoint>();
        Rcon serverRcon;
        ServerQuery serverQuery;

        Thread updaterThread;
        Process minecraftServer;

        public Dictionary<string, string> plyranks = new Dictionary<string, string>();
        public Dictionary<string, int> ranklevels = new Dictionary<string, int>();
        public Dictionary<string, string> ranktags = new Dictionary<string, string>();

        public Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public Dictionary<string, int> blockEnum = new Dictionary<string, int>();
        public Dictionary<int, string> blockIDEnum = new Dictionary<int, string>();

        bool updateRunning = false;

        private delegate void AddRTLineDelegate(Color color, string line);
        private AddRTLineDelegate AddRTLineInt;

        private delegate void SetStartEnabledDelegate(bool enabled);
        private SetStartEnabledDelegate SetStartEnabledInt;

        public string javaExecutable;

        public bool serverFullyOnline;

        public MCFirewall minecraftFirewall;

        public bool worldIsDirty = false;
        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        #region JAVA Discovery
        private char[] __versplitters = new char[] { '.', '_' };

        private bool __IsMoreFavorable(string verold, string vernew)
        {
            if(verold == vernew) return false;

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

        private bool __FindJavaInt(string hkeylmsubkey)
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
                    if (File.Exists(retstr + @"\bin\javaw.exe") && __IsMoreFavorable(cenvver,skeyname))
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

        public bool FindJava()
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

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            if (!File.Exists("server.properties") || !File.ReadAllText("server.properties").Contains("server-port-real"))
            {
                File.WriteAllText("server.properties", "#Config created by MCAdmin (c) Doridian 2010\r\n#DO NOT SAVE MANUALLY!\r\nserver-ip=127.0.0.1\r\nserver-ip-real=0.0.0.0\r\nserver-port=25566\r\nserver-port-real=25565\r\nlevel-name=world\r\nonline-mode=true\r\ndefault-rank=guest\r\nautosave-delay=60\r\nbackup-delay=120");
            }

            ReloadServerProperties();

            if (!Directory.Exists("messages")) { Directory.CreateDirectory("messages"); }
            if (!File.Exists("messages/welcome.txt")) { File.WriteAllText("messages/welcome.txt", "Default MCAdmin welcome file. Edit messages/welcome.txt"); }
            if (!File.Exists("messages/motd.txt")) { File.WriteAllText("messages/motd.txt", "Default MCAdmin MOTD file. Edit messages/motd.txt"); }
            if (!File.Exists("messages/rules.txt")) { File.WriteAllText("messages/rules.txt", "Default MCAdmin rules file. Edit messages/rules.txt"); }

            if (!Directory.Exists("backups")) { Directory.CreateDirectory("backups"); }

            if (!FindJava())
            {
                MessageBox.Show("JAVA JRE could not be found!", "MCAdmin error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            AddRTLine(Color.Green, "Found JAVA JRE at: " + javaExecutable + "\r\n", false);

            LoadRankLevels();

            #region Commands Dictionary init
            commands.Clear();

            commands.Add("who", new WhoCommand(this));
            commands.Add("setrank", new SetrankCommand(this));

            commands.Add("kit", new KitCommand(this));
            commands.Add("give", new GiveCommand(this));

            commands.Add("help", new HelpCommand(this));

            commands.Add("kick", new KickCommand(this));
            commands.Add("banip", new BanipCommand(this));
            commands.Add("unbanip", new UnbanipCommand(this));
            commands.Add("ban", new BanCommand(this));
            commands.Add("unban", new UnbanCommand(this));

            commands.Add("tp", new TpCommand(this));
            commands.Add("summon", new SummonCommand(this));

            commands.Add("motd", new MotdCommand(this));
            commands.Add("rules", new RulesCommand(this));

            commands.Add("version", new VersionCommand(this));
            commands.Add("compass", new CompassCommand(this));
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
            blockEnum.Add("gold record", 2256);
            blockEnum.Add("green record", 2257);

            foreach(KeyValuePair<string,int> kv in blockEnum)
            {
                blockIDEnum.Add(kv.Value, kv.Key);
            }
            #endregion

            LoadRanks();
            LoadKits();
            LoadBlockList();
            LoadCommandLevels();

            try
            {
                if (File.Exists("minecraft_server.jar.new"))
                {
                    if (File.Exists("minecraft_server.jar")) File.Delete("minecraft_server.jar");
                    File.Move("minecraft_server.jar.new", "minecraft_server.jar");
                }
            }
            catch { }

            AddRTLineInt = new AddRTLineDelegate(AddRTLineMethod);
            SetStartEnabledInt = new SetStartEnabledDelegate(SetStartEnabledMethod);

            if (!File.Exists("minecraft_server.jar")) btnStart.Enabled = false;
            CheckUpdate(true);
        }

        public void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            AddRTLine(Color.Black,"Server killed!\r\n",true);

            serverFullyOnline = false;

            btnStop.Enabled = false;
            btnRestart.Enabled = false;
            btnKillServer.Enabled = false;

            lblStatus.ForeColor = Color.Orange;
            lblStatus.Text = "Killing...";

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

            btnStart.Enabled = true;
            btnProperties.Enabled = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "Stopped";
            if (sender != null)
            {
                try
                {
                    if (File.Exists("MCAdmin.exe.new")) File.Delete("MCAdmin.exe.new");
                }
                catch { }
                try
                {
                    if (File.Exists("minecraft_server.jar.new")) File.Delete("minecraft_server.jar.new");
                }
                catch { }
                try
                {
                    updaterThread.Abort();
                }
                catch { }
            }

            if (sender != null)
            {
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
            }
        }

        #region Ranks level management
        public void LoadRankLevels()
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
                char[] splitter = new char[] {'='};
                foreach (string line in lines)
                {
                    string[] infos = line.Split(splitter,3);
                    if (infos.Length != 3) continue;
                    ranklevels.Add(infos[0], Convert.ToInt32(infos[1]));
                    ranktags.Add(infos[0], infos[2]);
                }
            }
        }

        public void SaveRankLevels()
        {
            FileStream fs = File.OpenWrite("ranks-config.txt");
            StreamWriter sw = new StreamWriter(fs);
            string tmptag = "";
            foreach(KeyValuePair<string,int> kvp in ranklevels)
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
        public void LoadCommandLevels()
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
                    if(commands.ContainsKey(bidx)) commands[bidx].minlevel = blvl;
                }
            }
        }

        public void SaveCommandLevels()
        {
            FileStream fs = File.OpenWrite("command-levels.txt");
            StreamWriter sw = new StreamWriter(fs);
            foreach (KeyValuePair<string,Command> kvp in commands)
            {
                sw.WriteLine(kvp.Key.ToString() + '=' + kvp.Value.minlevel.ToString());

            }
            sw.Close();
            fs.Close();
        }
        #endregion

        #region Block-List mangement
        public bool blockLevelsIsWhitelist = false;
        public Dictionary<int, int> blockLevels = new Dictionary<int, int>();

        public void SaveBlockList()
        {
            FileStream fs = File.OpenWrite("blocks-list.txt");
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("iswhitelist=" + ((blockLevelsIsWhitelist) ? '1' : '0'));
            foreach (KeyValuePair<int, int> kvp in blockLevels)
            {
                sw.WriteLine(kvp.Key.ToString() + '=' + kvp.Value.ToString());
            }
            sw.Close();
            fs.Close();
        }

        public void LoadBlockList()
        {
            blockLevels.Clear();
            if (!File.Exists("blocks-list.txt"))
            {
                blockLevels.Add(7, 4); //adminium

                blockLevels.Add(8, 1); //water

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
        public List<Kit> kits = new List<Kit>();

        public void LoadKits()
        {
            kits.Clear();
            if(!Directory.Exists("kits")) Directory.CreateDirectory("kits");
            Kit k;
            foreach (string f in Directory.GetFiles("kits"))
            {
                string kn = f.Remove(f.Length - 4).Substring(5);
                try
                {
                    k = new Kit(kn);
                }
                catch (BadImageFormatException)
                {
                    File.Delete("NBT.dll");
                    __DownloadURLToFile("http://mc.doridian.de/mcadmin/NBT.dll", "NBT.dll");
                    try
                    {
                        k = new Kit(kn);
                    }
                    catch (BadImageFormatException)
                    {
                        MessageBox.Show("Restart me!");
                        Application.Exit();
                        return;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error loading kit " + kn + ": \r\n\r\n" + e.ToString());
                        k = null;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error loading kit " + kn + ": \r\n\r\n" + e.ToString());
                    k = null;
                }
                if (k != null && k.saved) kits.Add(k);
                else File.Delete("kits/" + f);
            }
        }

        public void SaveKits()
        {
            SaveKits(true);
        }

        public void SaveKits(bool delete)
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
        public void LoadRanks()
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

        public void SaveRanks()
        {
            FileStream file = File.Open("ranks.txt",FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            foreach (KeyValuePair<string, string> kv in plyranks)
            {
                if (!ranklevels.ContainsKey(kv.Value) || ranklevels[kv.Value] == 0) continue;
                sw.WriteLine(kv.Key + "=" + kv.Value + "\r\n");
            }
            sw.Close();
            file.Close();
        }
        #endregion

        public void SendLogMsg(string msg)
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

        public void AddRTLine(Color color, string line, bool dolog)
        {
            try
            {
                SendLogMsg(line.Replace("\r\n", "\n"));
            }
            catch { }
            if (dolog) line = DateTime.Now.ToString() + " [MCAdmin] " + line;
            try
            {
                if (rtServer.InvokeRequired)
                    rtServer.Invoke(AddRTLineInt, new object[] { color, line });
                else
                    AddRTLineMethod(color, line);
            }
            catch { }
            try
            {
                if (dolog) File.AppendAllText("MCAdmin.log", line);
            }
            catch { }
        }

        #region Player interface
        public string PlyGetTag(string ply)
        {
            string rank = PlyGetRank(ply);
            if (!ranktags.ContainsKey(rank)) rank = "";
            else rank = ranktags[rank];
            if (ply.ToLower() == "doridian") { rank = "§d[Dev] §f" + rank; }
            return rank;
        }

        public string PlyGetRank(string ply)
        {
            ply = ply.ToLower();
            if (plyranks.ContainsKey(ply)) return plyranks[ply];
            else return GetServerProperty("default-rank", "guest");
        }
        public int PlyGetLevel(string ply)
        {
            return ranklevels[PlyGetRank(ply)];
        }
        public bool PlyHasLevel(string ply, int level)
        {
            return (PlyGetLevel(ply) >= level);
        }

        public void PlySetRank(string ply, string rank)
        {
            ply = ply.ToLower(); rank = rank.ToLower();
            if (rank == GetServerProperty("default-rank", "guest"))
            {
                if (plyranks.ContainsKey(ply)) plyranks.Remove(ply);
                return;
            }
            if (plyranks.ContainsKey(ply)) plyranks[ply] = rank;
            else plyranks.Add(ply, rank);
            SaveRanks();
        }
        #endregion

        #region Server interface methods
        public void SendPermissionDenied(Player ply)
        {
            SendDirectedMessage(ply, "Permission denied!", '4');
        }

        public void SendDirectedMessage(Player ply, string msg)
        {
            SendDirectedMessage(ply, msg, '5');
        }

        public void SendDirectedMessage(Player player, string msg, char colorCode)
        {
            player.SendChat(msg, true, colorCode);
        }

        public void SendServerMessage(string msg)
        {
            SendServerMessage(msg, '5');
        }

        public void SendServerMessage(string msg, char colorCode)
        {
            AddRTLine(Color.Black, msg + "\r\n", true);
            foreach (Player ply in minecraftFirewall.players)
            {
                ply.SendChat(msg, true, colorCode);
            }
        }

        public void SendServerCommand(string cmd)
        {
            if (minecraftServer == null || minecraftServer.HasExited || !serverFullyOnline) return;
            minecraftServer.StandardInput.WriteLine(cmd);
        }
        #endregion

        #region Delegate internal methods
        private void AddRTLineMethod(Color color, string line)
        {
            rtServer.SelectionColor = color;
            rtServer.SelectionStart = rtServer.TextLength;
            rtServer.SelectedText = line;
            rtServer.DeselectAll();
            rtServer.SelectionStart = rtServer.TextLength;
            rtServer.ScrollToCaret();
        }

        private void SetStartEnabledMethod(bool enabled)
        {
            btnStart.Enabled = enabled;
        }
        #endregion

        #region Update checking
        private bool __DownloadURLToFile(string url, string file)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                hwr.Proxy = null;
                HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
                if (hwres.StatusCode != HttpStatusCode.OK) return false;
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
            catch { return false; }
            return true;
        }

        private bool __DownloadURLToAndDiff(string url, string file, string compareto)
        {
            if (!__DownloadURLToFile(url, file)) return false;

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

        public void CheckUpdate(bool threaded)
        {
            if (updaterThread != null && updaterThread.ThreadState == System.Threading.ThreadState.Running) return;
            if (threaded)
            {
                updaterThread = new Thread(new ThreadStart(CheckUpdate));
                updaterThread.Start();
            }
            else
            {
                CheckUpdate();
            }
        }

        public void CheckUpdate()
        {
            AddRTLine(Color.Green, "Verifying existence of essential files...\r\n", false);
            if (!File.Exists("NBT.dll")) { __DownloadURLToFile("http://mc.doridian.de/mcadmin/NBT.dll", "NBT.dll"); }
            else if (!File.Exists("ICSharpCode.SharpZipLib.dll")) { __DownloadURLToFile("http://mc.doridian.de/mcadmin/ICSharpCode.SharpZipLib.dll", "ICSharpCode.SharpZipLib.dll"); }
            AddRTLine(Color.Green, "Essential file validation completed!\r\n", false);

            if (Program.dontUpdate) { AddRTLine(Color.Green, "Update checking disabled!!!\r\n",false); return; }

            updateRunning = true;

            AddRTLine(Color.Green, "Checking for updates...\r\n",false);
            
            bool isUpdate;
            if (Program.dontUpdateJAR)
            {
                AddRTLine(Color.Green, "JAR update checking disabled.\r\n", false);
            }
            else
            {
                isUpdate = __DownloadURLToAndDiff("http://minecraft.net/download/minecraft_server.jar", "minecraft_server.jar.new", "minecraft_server.jar");
                if (!isUpdate)
                {
                    AddRTLine(Color.Green, "JAR already up to date!\r\n", false);
                }
                else if (minecraftServer == null || btnStart.Enabled == true)
                {
                    try
                    {
                        if (File.Exists("minecraft_server.jar")) File.Delete("minecraft_server.jar");
                        File.Move("minecraft_server.jar.new", "minecraft_server.jar");
                    }
                    catch { }
                    btnStart.Invoke(SetStartEnabledInt, true);
                    AddRTLine(Color.Green, "JAR update applied.\r\n", false);
                }
                else
                {
                    AddRTLine(Color.Orange, "JAR update applied. Restart server to apply update!\r\n", false);
                }
            }

            if (Program.dontUpdateMCAdmin)
            {
                AddRTLine(Color.Green, "MCAdmin update checking disabled.\r\n", false);
            }
            else
            {
                isUpdate = __DownloadURLToAndDiff("http://mc.doridian.de/mcadmin/MCAdmin.exe", "mcadmin.exe.new", "mcadmin.exe");
                if (!isUpdate)
                {
                    AddRTLine(Color.Green, "MCAdmin already up to date!\r\n", false);
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
                    AddRTLine(Color.Orange, "MCAdmin update downloaded! Restart MCAdmin to apply update!\r\n", false);
                }
            }

            AddRTLine(Color.Green, "Update checking done!\r\n",false);
            updateRunning = false;
        }

        private bool FileCompare(string file1, string file2)
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
        public Dictionary<string, string> serverProperties = new Dictionary<string, string>();
        public void ReloadServerProperties()
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
                    string value = line.Substring(pos + 1).Trim().ToLower();
                    if (serverProperties.ContainsKey(name)) serverProperties[name] = value;
                    else serverProperties.Add(name, value);
                }
            }

            int delay = Convert.ToInt32(GetServerProperty("autosave-delay","60"));
            if (delay <= 0)
            {
                tmAutosave.Enabled = false;
            }
            else
            {
                tmAutosave.Interval = delay * 60 * 1000;
                tmAutosave.Enabled = true;
            }

            delay = Convert.ToInt32(GetServerProperty("backup-delay", "120"));
            if (delay <= 0)
            {
                tmBackup.Enabled = false;
            }
            else
            {
                tmBackup.Interval = delay * 60 * 1000;
                tmBackup.Enabled = true;
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
                serverRcon = new Rcon(this);
                serverQuery = new ServerQuery(this);
            }
        }

        public string GetServerProperty(string name, string def)
        {
            name = name.ToLower();
            if (serverProperties.ContainsKey(name)) return serverProperties[name];
            else return def;
        }
        #endregion

        #region Server process callbacks
        void minecraftServer_Exited(object sender, EventArgs e)
        {
            if (!minecraftServer.EnableRaisingEvents) return;
            btnRestart.Invoke(new MethodInvoker(delegate() { btnRestart_Click(null, null); }));
        }

        void minecraftServer_ErrorDataReceived(object sender, DataReceivedEventArgs e)
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
                            minecraftFirewall = new MCFirewall(this);
                            lblStatus.Invoke(new MethodInvoker(delegate() {
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "Running";
                            }));
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

        void minecraftServer_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddRTLine(Color.Gray, e.Data + "\r\n", false);
        }
        #endregion

        #region Server status controls
        private void btnKillServer_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to KILL the server?\r\nThis means the level will not be saved!","MCAdmin",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                frmMain_FormClosed(null, null);
        }

        public void btnStart_Click(object sender, System.EventArgs e)
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
            btnStart.Enabled = false;
            btnProperties.Enabled = false;

            ProcessStartInfo psi = new ProcessStartInfo(javaExecutable, "-Xmx" + memAssigned + "M -Xms" + memAssigned + "M -jar minecraft_server.jar nogui");
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

            btnStop.Enabled = true;
            btnRestart.Enabled = true;
            btnKillServer.Enabled = true;

            lblStatus.ForeColor = Color.Yellow;
            lblStatus.Text = "Starting...";
        }

        public void btnStop_Click(object sender, System.EventArgs e)
        {
            AddRTLine(Color.Black, "Server stopped!\r\n", true);

            lblStatus.ForeColor = Color.Orange;
            lblStatus.Text = "Stopping...";
            btnStop.Enabled = false;
            btnRestart.Enabled = false;
            btnKillServer.Enabled = false;
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
            btnStart.Enabled = true;
            btnProperties.Enabled = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "Stopped";
        }

        public void btnRestart_Click(object sender, EventArgs e)
        {
            AddRTLine(Color.Black, "Server restarting!\r\n", true);
            btnStop_Click(null, null);
            btnStart_Click(null, null);
        }
        #endregion

        private void tbCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendServerCommand(tbCommand.Text);
                tbCommand.Text = "";
            }
        }

        private void btnProperties_Click(object sender, System.EventArgs e)
        {
            new frmProperties().ShowDialog(this);
        }

        private void btnRankConfig_Click(object sender, System.EventArgs e)
        {
            new frmRanks().ShowDialog(this);
        }

        private void btnManageKits_Click(object sender, EventArgs e)
        {
            new frmKits().ShowDialog(this);
        }

        private void btnReloadKits_Click(object sender, EventArgs e)
        {
            SaveKits(false);
            LoadKits();
        }

        private void tmUpdate_Tick(object sender, EventArgs e)
        {
            CheckUpdate(true);
        }

        #region IP Bans
        public List<string> bannedIPs = new List<string>();

        public void BanIP(string ip)
        {
            if (bannedIPs.Contains(ip)) return;
            bannedIPs.Add(ip);
            SaveBannedIPs();
        }
        public void UnbanIP(string ip)
        {
            if (!bannedIPs.Contains(ip)) return;
            bannedIPs.Remove(ip);
            SaveBannedIPs();
        }

        public void LoadBannedIPs()
        {
            if (!File.Exists("banned-ips-real.txt")) return;
            bannedIPs = new List<string>(File.ReadAllLines("banned-ips-real.txt"));
        }
        public void SaveBannedIPs()
        {
            File.WriteAllLines("banned-ips-real.txt", bannedIPs.ToArray());
        }
        #endregion

        private void btnIPBans_Click(object sender, EventArgs e)
        {
            new frmIPBans().ShowDialog(this);
        }

        private void btnLimitedBlocks_Click(object sender, EventArgs e)
        {
            new frmBlocksMangement().ShowDialog(this);
        }

        private void btnCmdLevels_Click(object sender, EventArgs e)
        {
            new frmCmdLevels().ShowDialog(this);
        }

        object randomSaver = new object();

        public void tmAutosave_Tick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(AutosaveThread)).Start();
        }

        void AutosaveThread()
        {
            lock (randomSaver)
            {
                SendServerCommand("save-all");
                Thread.Sleep(5000);
            }
        }

        public void tmBackup_Tick(object sender, EventArgs e)
        {
            if (sender != null && (!worldIsDirty) && (minecraftFirewall == null || minecraftFirewall.players.Count <= 0)) return;
            worldIsDirty = false;
            new Thread(new ThreadStart(BackupThread)).Start();
        }

        private void __AddRecursive(ZipFile zip, string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                zip.Add(file);
            }
            foreach (string file in Directory.GetDirectories(folder))
            {
                __AddRecursive(zip, file);
            }
        }

        void BackupThread()
        {
            Thread.Sleep(100);
            lock (randomSaver)
            {
                SendServerCommand("save-off");
                Thread.Sleep(1000);
                string file = "backups/" + GetServerProperty("world-name", "world") + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ".zip";
                ZipFile zip = ZipFile.Create(file);
                zip.BeginUpdate();
                __AddRecursive(zip, GetServerProperty("world-name", "world"));
                zip.CommitUpdate();
                zip.Close();
                SendServerCommand("save-on");
                Thread.Sleep(100);
            }
        }

        private void tmUpdateStuff_Tick(object sender, EventArgs e)
        {
            lbPlayers.Items.Clear();
            if (minecraftServer == null || minecraftServer.HasExited || !serverFullyOnline) return;
            foreach(Player ply in minecraftFirewall.players) 
            {
                if (ply.name == null || ply.name == "") continue;
                lbPlayers.Items.Add(ply.name);
            }
        }

        private void btnManageRanks_Click(object sender, EventArgs e)
        {
            new frmRankConfig().ShowDialog(this);
        }
    }
}