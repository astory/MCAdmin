using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace MCAdmin
{
    public partial class frmKitShare : Form
    {
        Thread currentKitLoader;
        private string __DownloadURL(string url)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                hwr.Proxy = null;
                HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
                if (hwres.StatusCode != HttpStatusCode.OK) return null;
                Stream str = hwres.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                string ret = sr.ReadToEnd();
                sr.Close();
                str.Close();
                hwres.Close();
                return ret;
            }
            catch { return null; }
        }

        void LoadNewListThread()
        {
            string listData = __DownloadURL("http://internal.mcadmin.eu/kitshare/list.php");
            if (listData == null) { lbKits.Invoke(new MethodInvoker(delegate() { lbKits.Items.Clear(); lbKits.Items.Add("Could not connect!"); })); return; }
            lbKits.Invoke(new MethodInvoker(delegate()
            {
                lbKits.Items.Clear();
                string[] stuff = listData.Split(new char[] { '\n' });
                foreach (string kit in stuff)
                {
                    if(kit.Length > 0) lbKits.Items.Add(kit);
                }
                listAvailable = true;
            }));
            currentKitLoader = null;
        }

        void LoadKitThread(object kitname_x)
        {
            string kitname = (string)kitname_x;
            bool downloadsucc = parentparent.DownloadURLToFile("http://internal.mcadmin.eu/kitshare/kits/" + kitname + ".kit", "tmpkit.kit");
            Kit thiskit = null;
            if (downloadsucc)
            {
                thiskit = new Kit(kitname, "tmpkit.kit");
                File.Delete("tmpkit.kit");
            }
            lvItems.Invoke(new MethodInvoker(delegate()
            {
                lvItems.Items.Clear();
                if (downloadsucc && thiskit != null)
                {
                    foreach (KeyValuePair<int, int> kv in thiskit.items)
                    {
                        lvItems.Items.Add(new ListViewItem(new string[] { kv.Key.ToString(), parentparent.blockIDEnum[kv.Key], kv.Value.ToString() }));
                    }
                    btnDownload.Enabled = true;
                    lvItems.Enabled = true;
                }
            }));
        }

        bool listAvailable = false;
        frmMain parentparent;
        //frmKits parent;

        public frmKitShare()
        {
            InitializeComponent();
        }

        private void frmKitShare_Load(object sender, EventArgs e)
        {
            parentparent = (frmMain)this.Owner;
            //parent = (frmKits)this.Owner;
            btnRefresh_Click(null, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            AbortKitLoading();
            listAvailable = false;
            lbKits.Items.Clear();
            lbKits.Items.Add("Loading...");
            new Thread(new ThreadStart(LoadNewListThread)).Start();
        }

        private void lbKits_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!listAvailable) return;
            AbortKitLoading();
            currentKitLoader = new Thread(new ParameterizedThreadStart(LoadKitThread));
            currentKitLoader.Start(lbKits.SelectedItem);
        }

        private void AbortKitLoading()
        {
            lvItems.Enabled = false;
            btnDownload.Enabled = false;
            try
            {
                if (currentKitLoader != null) currentKitLoader.Abort();
            }
            catch { }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            new Thread(new ParameterizedThreadStart(KitDownloader)).Start(lbKits.SelectedItem);
        }

        void KitDownloader(object kitname_x)
        {
            string kitname = (string)kitname_x;
            parentparent.DownloadURLToFile("http://internal.mcadmin.eu/kitshare/kits/" + kitname + ".kit", Kit.GetKitFile(kitname));
            parentparent.LoadKits();
            MessageBox.Show("Download complete!");
        }
    }
}
