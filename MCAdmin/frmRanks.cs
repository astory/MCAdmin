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
    public partial class frmRanks : Form
    {
        frmMain parent;
        int selecteditem = -1;

        List<string> toUnban = new List<string>();

        public frmRanks()
        {
            InitializeComponent();
        }

        private void frmRanks_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            foreach (KeyValuePair<string, string> kv in Program.plyranks)
            {
                lvRanks.Items.Add(new ListViewItem(new string[] { kv.Key, kv.Value }));
            }

            foreach (string rankname in Program.ranklevels.Keys)
            {
                cbRank.Items.Add(rankname);
            }

            if (Program.minecraftFirewall != null)
            {
                foreach (Player ply in Program.minecraftFirewall.players)
                {
                    if (ply.name == null || ply.name == "") continue;
                    cbPlayer.Items.Add(ply.name);
                }
            }

            cbRank.SelectedIndex = 0;
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            if (selecteditem < 0)
            {
                foreach (ListViewItem lvi in lvRanks.Items)
                {
                    if (lvi.SubItems[0].Text == cbPlayer.Text.ToLower()) selecteditem = lvi.Index;
                }
            }
            if (selecteditem >= 0) 
            {
                ListViewItem lvi = lvRanks.Items[selecteditem];
                if (lvi.SubItems[1].Text == "banned") toUnban.Add(cbPlayer.Text.ToLower());
                lvi.SubItems[0].Text = cbPlayer.Text.ToLower();
                lvi.SubItems[1].Text = cbRank.Text.ToLower();
            }
            else
            {
                lvRanks.Items.Add(new ListViewItem(new string[] { cbPlayer.Text.ToLower(), cbRank.Text.ToLower() }));
            }
            cbPlayer.Text = "";
            cbRank.SelectedIndex = 0;
            selecteditem = -1;
        }

        private void lvRanks_DoubleClick(object sender, EventArgs e)
        {
            if (lvRanks.SelectedItems.Count <= 0) return;
            ListViewItem lvi = lvRanks.SelectedItems[0];
            selecteditem = lvRanks.SelectedIndices[0];
            cbPlayer.Text = lvi.SubItems[0].Text;
            cbRank.Text = lvi.SubItems[1].Text;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (selecteditem >= 0) lvRanks.Items.RemoveAt(selecteditem);
            cbPlayer.Text = "";
            cbRank.SelectedIndex = 0;
            selecteditem = -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.plyranks.Clear();
            foreach (ListViewItem lvi in lvRanks.Items)
            {
                Program.plyranks.Add(lvi.SubItems[0].Text, lvi.SubItems[1].Text);
            }
            Program.SaveRanks();

            foreach (string str in toUnban)
            {
                Heartbeats.MasterBans.UnbanUser(str, "CONSOLE");
            }

            this.Close();
        }
    }
}
