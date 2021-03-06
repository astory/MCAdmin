﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCAdmin
{
    public partial class frmWelcome : Form
    {
        public frmWelcome()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Program.SetServerProperty("server-name", tbName.Text);
            if(tbOwner.Text != "") Program.PlySetRank(tbOwner.Text, "admin");
            if (cbWhitelist.Checked)
            {
                Program.SetServerProperty("default-rank", "banned");
            }
            if (!Program.serverProperties.ContainsKey("server-port")) Program.SetServerProperty("server-port", "25566");
            if (!Program.serverProperties.ContainsKey("server-port-real")) Program.SetServerProperty("server-port-real", "25565");

            Program.mbansEnable = cbBans.Checked;
            Program.mbansSubmit = cbBans.Checked;
            Program.mlistEnable = cbList.Checked;
            Program.mlistSendNames = cbList.Checked;
            Program.mlistSendRanks = cbList.Checked;
            Program.SaveMainConfig();
            Program.SaveServerProperties();
            this.Close();
        }
    }
}
