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
    public partial class frmRankConfig : Form
    {
        frmMain parent;
        int selecteditem = -1;

        public frmRankConfig()
        {
            InitializeComponent();
        }

        private void frmRanks_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            string tmptag = "";
            foreach (KeyValuePair<string, int> kv in parent.ranklevels)
            {
                if (parent.ranktags.ContainsKey(kv.Key)) tmptag = parent.ranktags[kv.Key];
                else tmptag = "";
                lvRanks.Items.Add(new ListViewItem(new string[] { kv.Key, kv.Value.ToString(), tmptag }));
            }
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            if (selecteditem < 0)
            {
                foreach (ListViewItem lvi in lvRanks.Items)
                {
                    if (lvi.SubItems[0].Text == tbName.Text.ToLower()) selecteditem = lvi.Index;
                }
            }
            if (selecteditem >= 0) 
            {
                ListViewItem lvi = lvRanks.Items[selecteditem];
                lvi.SubItems[0].Text = tbName.Text.ToLower();
                lvi.SubItems[1].Text = numLevel.Value.ToString();
                lvi.SubItems[2].Text = tbTag.Text;
            }
            else
            {
                lvRanks.Items.Add(new ListViewItem(new string[] { tbName.Text.ToLower(), numLevel.Value.ToString(), tbTag.Text }));
            }
            tbName.Text = "";
            numLevel.Value = 0;
            tbTag.Text = "";
            selecteditem = -1;
        }

        private void lvRanks_DoubleClick(object sender, EventArgs e)
        {
            if (lvRanks.SelectedItems.Count <= 0) return;
            ListViewItem lvi = lvRanks.SelectedItems[0];
            selecteditem = lvRanks.SelectedIndices[0];
            tbName.Text = lvi.SubItems[0].Text;
            numLevel.Value = Convert.ToInt32(lvi.SubItems[1].Text);
            tbTag.Text = lvi.SubItems[2].Text;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (selecteditem >= 0) lvRanks.Items.RemoveAt(selecteditem);
            tbName.Text = "";
            numLevel.Value = 0;
            tbTag.Text = "";
            selecteditem = -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parent.ranklevels.Clear();
            parent.ranktags.Clear();
            foreach (ListViewItem lvi in lvRanks.Items)
            {
                parent.ranklevels.Add(lvi.SubItems[0].Text, Convert.ToInt32(lvi.SubItems[1].Text));
                parent.ranktags.Add(lvi.SubItems[0].Text, lvi.SubItems[2].Text);
            }
            parent.SaveRankLevels();

            this.Close();
        }
    }
}
