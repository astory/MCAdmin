using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCAdmin
{
    public partial class frmIPBans : Form
    {
        frmMain parent;

        public frmIPBans()
        {
            InitializeComponent();
        }

        private void frmIPBans_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            Program.LoadBannedIPs();

            foreach (string str in Program.bannedIPs)
                lbIPBans.Items.Add(str);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.bannedIPs.Clear();

            foreach (object item in lbIPBans.Items)
                Program.bannedIPs.Add((string)item);

            Program.SaveBannedIPs();
            this.Close();
        }

        private void btnAddBan_Click(object sender, EventArgs e)
        {
            string str = tbBanIP.Text;
            tbBanIP.Text = "";

            foreach (object item in lbIPBans.Items)
                if ((string)item == str) return;

            lbIPBans.Items.Add(str);
        }

        private void lbIPBans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lbIPBans.SelectedItems.Count > 0)
            {
                object[] selitmes = new object[lbIPBans.SelectedItems.Count];
                lbIPBans.SelectedItems.CopyTo(selitmes,0);
                foreach (object obj in selitmes)
                {
                    lbIPBans.Items.Remove(obj);
                }
            }
        }
    }
}
