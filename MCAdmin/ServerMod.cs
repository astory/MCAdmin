using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;

namespace MCAdmin
{
    class ServerMod
    {
        public string name; public string link; public string author; public string downloadURL;
        Thread updaterThread;
        public ServerMod(string m_name, string m_link, string m_author, string m_downloadURL)
        {
            name = m_name;
            author = m_author;
            link = m_link;
            downloadURL = m_downloadURL;
        }

        public void CheckUpdate()
        {
            if (updaterThread != null && updaterThread.IsAlive) return;
            updaterThread = new Thread(new ThreadStart(CheckUpdateT));
            updaterThread.Start();
        }

        private void CheckUpdateT()
        {
            if (!this.IsInstalled()) return;
            bool isUpdate = false;
            if (Program.DownloadURLToAndDiff(downloadURL, "mods/" + name + ".zip.new", "mods/" + name + ".zip"))
            {
                try
                {
                    if (File.Exists("mods/" + name + ".zip")) File.Delete("mods/" + name + ".zip");
                    File.Move("mods/" + name + ".zip.new", "mods/" + name + ".zip");
                    isUpdate = true;
                }
                catch
                {
                    Program.AddRTLine(Color.Red, "Error updating/installing mod " + name + "\r\n", false);
                }
            }
            if (isUpdate || !Directory.Exists("mods/" + name))
            {
                try
                {
                    if (Directory.Exists("mods/" + name))
                    {
                        Directory.Delete("mods/" + name, true);
                        Directory.CreateDirectory("mods/" + name);
                    }
                }
                catch { }

                FastZip fastZip = new FastZip();
                fastZip.ExtractZip("mods/" + name + ".zip", "mods/" + name, "");

                Program.AddRTLine(Color.Orange, "Updated/Installed mod " + name + "\r\n", false);
            }
        }

        public bool IsInstalled()
        {
            return Directory.Exists("mods/" + name);
        }

        public void Install()
        {
            if (this.IsInstalled()) return;

            if (updaterThread != null && updaterThread.IsAlive) return;

            Program.AddRTLine(Color.Orange, "Started installation of mod " + name + ". Wait for confirmation!!!\r\n", false);

            Directory.CreateDirectory("mods/" + name);
            CheckUpdate();
        }

        public void Uninstall()
        {
            if (!this.IsInstalled()) return;

            if (updaterThread != null && updaterThread.IsAlive) return;

            try
            {
                updaterThread.Abort();
            }
            catch { }

            Directory.Delete("mods/" + name, true);
            try
            {
                File.Delete("mods/" + name + ".zip");
            }
            catch { }
            try
            {
                File.Delete("mods/" + name + ".zip.new");
            }
            catch { }
        }
    }
}
