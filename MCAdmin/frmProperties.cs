using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;

namespace MCAdmin
{
    public partial class frmProperties : Form
    {
        frmMain parent;

        public frmProperties()
        {
            InitializeComponent();
        }


        private void frmProperties_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;

            parent.ReloadServerProperties();

            tbPort.Text = parent.GetServerProperty("server-port-real","25565");
            tbLevel.Text = parent.GetServerProperty("level-name","world");
            tbIntPort.Text = parent.GetServerProperty("server-port", "25566");
            numMemory.Value = Convert.ToInt32(parent.GetServerProperty("assigned-memory", "1024"));
            numAS.Value = Convert.ToInt32(parent.GetServerProperty("autosave-delay", "60"));
            numBackup.Value = Convert.ToInt32(parent.GetServerProperty("backup-delay", "120"));

            tbRCONPass.Text = parent.GetServerProperty("rcon-pass", "changeme");
            tbRCONPort.Text = parent.GetServerProperty("rcon-port", "25567");
            cbRCONEnable.Checked = (parent.GetServerProperty("rcon-enable", "false").ToLower() == "true");

            foreach (string rankname in parent.ranklevels.Keys)
            {
                cbDefRank.Items.Add(rankname);
            }
            cbDefRank.Text = parent.GetServerProperty("default-rank", "guest");

            cbIP.Items.Add("0.0.0.0");

            cbOnline.Checked = (parent.GetServerProperty("online-mode", "true").ToLower() == "true");

            foreach(NetworkInterface iface in  NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (IPAddressInformation ipaddrinfo in iface.GetIPProperties().UnicastAddresses)
                {
                    IPAddress ipaddr = ipaddrinfo.Address;
                    if ((!IPAddress.IsLoopback(ipaddr)) && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        cbIP.Items.Add(ipaddr.ToString());
                    }
                }
            }


            cbIP.Text = parent.GetServerProperty("server-ip-real", "0.0.0.0");
            if (cbIP.Text == "") cbIP.Text = "0.0.0.0";

            cbRCONEnable_CheckedChanged(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbIntPort.Text == tbPort.Text || (cbRCONEnable.Checked && (tbIntPort.Text == tbRCONPort.Text || tbPort.Text == tbRCONPort.Text)))
            {
                MessageBox.Show("Ports may never be equal!!!");
                return;
            }
            File.WriteAllText("server.properties", tbPreview.Text);
            parent.ReloadServerProperties();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RefreshPreview()
        {
            tbPreview.Text = "#Config created by MCAdmin (c) Doridian 2010\r\n#DO NOT SAVE MANUALLY!\r\nserver-ip=127.0.0.1\r\nserver-ip-real=" + cbIP.Text + "\r\nserver-port=" + tbIntPort.Text + "\r\nserver-port-real=" + tbPort.Text + "\r\nlevel-name=" + tbLevel.Text + "\r\nassigned-memory=" + numMemory.Value.ToString() + "\r\nonline-mode=" + ((cbOnline.Checked) ? "true" : "false") + "\r\ndefault-rank=" + cbDefRank.Text + "\r\nautosave-delay=" + numAS.Value.ToString() + "\r\nbackup-delay=" + numBackup.Value.ToString() + "\r\nrcon-enable="+((cbRCONEnable.Checked) ? "true" : "false")+"\r\nrcon-port="+tbRCONPort.Text+"\r\nrcon-pass="+tbRCONPass.Text;
        }

        private void ShowToolTipError(Control ctrl, string title, string text)
        {
            ttBase.Hide(this);
            ttBase.ToolTipIcon = ToolTipIcon.Error;
            ttBase.ToolTipTitle = title;
            ttBase.Show(text, this, ctrl.Location.X + (ctrl.Size.Width / 2), ctrl.Location.Y - 50, 2000);
        }

        private void numericStuff_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                ShowToolTipError(tbIntPort, "Invalid input", "Port may only consist of numbers");
                e.Handled = true;
            }
        }

        private void event_RefreshPreview(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void cbRCONEnable_CheckedChanged(object sender, EventArgs e)
        {
            lblRCONPass.Enabled = cbRCONEnable.Checked;
            lblRCONPort.Enabled = cbRCONEnable.Checked;
            tbRCONPass.Enabled = cbRCONEnable.Checked;
            tbRCONPort.Enabled = cbRCONEnable.Checked;
            RefreshPreview();
        }
    }
}
