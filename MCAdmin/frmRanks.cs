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
    public partial class frmRanks : Form
    {
        frmMain parent;
        int selecteditem = -1;

        public frmRanks()
        {
            InitializeComponent();
        }

        private void frmRanks_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            foreach (KeyValuePair<string, string> kv in parent.plyranks)
            {
                lvRanks.Items.Add(new ListViewItem(new string[] { kv.Key, kv.Value }));
            }

            foreach (string rankname in parent.ranklevels.Keys)
            {
                cbRank.Items.Add(rankname);
            }

            if (parent.minecraftFirewall != null)
            {
                foreach (Player ply in parent.minecraftFirewall.players)
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
            parent.plyranks.Clear();
            foreach (ListViewItem lvi in lvRanks.Items)
            {
                parent.plyranks.Add(lvi.SubItems[0].Text, lvi.SubItems[1].Text);
            }
            parent.SaveRanks();
            this.Close();
        }
    }
}
