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
using System.Runtime.InteropServices;
using MiscUtil.Conversion;
using System.Net.Sockets;

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

            Program.ReloadServerProperties();

            tbPort.Text = Program.GetServerProperty("server-port-real","25565");
            tbLevel.Text = Program.GetServerProperty("level-name","world");
            tbIntPort.Text = Program.GetServerProperty("server-port", "25566");
            tbSrvName.Text = Program.GetServerProperty("server-name", "MCAdmin Server");
            numMemory.Value = Convert.ToInt32(Program.GetServerProperty("assigned-memory", "1024"));
            numAS.Value = Convert.ToInt32(Program.GetServerProperty("autosave-delay", "60"));
            numBackup.Value = Convert.ToInt32(Program.GetServerProperty("backup-delay", "120"));
            numMaxPlayers.Value = Convert.ToInt32(Program.GetServerProperty("max-players", "20"));

            tbRCONPass.Text = Program.GetServerProperty("rcon-pass", "changeme");
            tbRCONPort.Text = Program.GetServerProperty("rcon-port", "25567");
            cbRCONEnable.Checked = (Program.GetServerProperty("rcon-enable", "false").ToLower() == "true");

            foreach (string rankname in Program.ranklevels.Keys)
            {
                cbDefRank.Items.Add(rankname);
            }
            cbDefRank.Text = Program.GetServerProperty("default-rank", "guest");

            cbIP.Items.Add("0.0.0.0");

            cbOnline.Checked = (Program.GetServerProperty("online-mode", "true").ToLower() == "true");

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


            cbIP.Text = Program.GetServerProperty("server-ip-real", "0.0.0.0");
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
            Program.ReloadServerProperties();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RefreshPreview()
        {
            tbPreview.Text = "#Config created by MCAdmin (c) Doridian 2010\r\n#DO NOT SAVE MANUALLY!\r\nserver-ip=127.0.0.1\r\nserver-ip-real=" + cbIP.Text + "\r\nserver-port=" + tbIntPort.Text + "\r\nserver-port-real=" + tbPort.Text + "\r\nlevel-name=" + tbLevel.Text + "\r\nassigned-memory=" + numMemory.Value.ToString() + "\r\nonline-mode=" + ((cbOnline.Checked) ? "true" : "false") + "\r\nmax-players="+numMaxPlayers.Value.ToString()+"\r\ndefault-rank=" + cbDefRank.Text + "\r\nautosave-delay=" + numAS.Value.ToString() + "\r\nbackup-delay=" + numBackup.Value.ToString() + "\r\nrcon-enable="+((cbRCONEnable.Checked) ? "true" : "false")+"\r\nrcon-port="+tbRCONPort.Text+"\r\nrcon-pass="+tbRCONPass.Text+"\r\nserver-name="+tbSrvName.Text;
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
                ShowToolTipError((Control)sender, "Invalid input", "Port may only consist of numbers");
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

        private void btnAutoDetect_Click(object sender, EventArgs e)
        {
            try
            {
                Socket testsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                testsock.ReceiveTimeout = 1000;
                testsock.SendTimeout = 1000;
                testsock.Connect("mcadmin.eu", 80);
                string iptouse = IPAddress.Parse(((IPEndPoint)testsock.LocalEndPoint).Address.ToString()).ToString();
                testsock.Close();
                cbIP.Text = iptouse;
                MessageBox.Show("Auto-detect completed!\nBest interface: " + iptouse);
            }
            catch { MessageBox.Show("Sorry, error during auto-detection :("); }
        }
    }
}
