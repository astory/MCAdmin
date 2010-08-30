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
    public partial class frmBlocksMangement : Form
    {
        frmMain parent;

        public frmBlocksMangement()
        {
            InitializeComponent();
        }

        private void lvBlocks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lvBlocks.SelectedItems.Count > 0)
            {
                ListViewItem[] selitmes = new ListViewItem[lvBlocks.SelectedItems.Count];
                lvBlocks.SelectedItems.CopyTo(selitmes, 0);
                foreach (ListViewItem obj in selitmes)
                {
                    lvBlocks.Items.Remove(obj);
                }
            }
        }

        private void frmBlocksMangement_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            parent.LoadBlockList();
            rbWhitelist.Checked = parent.blockLevelsIsWhitelist;
            foreach (KeyValuePair<int, int> kvp in parent.blockLevels)
            {
                lvBlocks.Items.Add(new ListViewItem(new string[] { kvp.Key.ToString(), parent.blockIDEnum[kvp.Key], kvp.Value.ToString() }));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parent.blockLevels.Clear();
            foreach (ListViewItem lvi in lvBlocks.Items)
            {
                parent.blockLevels.Add(Convert.ToInt32(lvi.SubItems[0].Text), Convert.ToInt32(lvi.SubItems[2].Text));
            }
            parent.blockLevelsIsWhitelist = rbWhitelist.Checked;
            parent.SaveBlockList();
            this.Close();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            new frmBlockManegementAdd().ShowDialog(this);
        }

        private void lvBlocks_DoubleClick(object sender, EventArgs e)
        {
            if (lvBlocks.SelectedItems.Count <= 0) return;
            new frmBlockManegementAdd(lvBlocks.SelectedItems[0]).ShowDialog(this);
        }

        private void btnDelItem_Click(object sender, EventArgs e)
        {
            if (lvBlocks.SelectedItems.Count <= 0) return;
            lvBlocks.SelectedItems[0].Remove();
        }
    }
}
