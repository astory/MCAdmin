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
    public partial class frmBlockManegementAdd : Form
    {
        frmMain parentparent;
        frmBlocksMangement parent;
        ListViewItem boundItem;

        int cbRankDef = 0;


        public frmBlockManegementAdd(ListViewItem item)
            : this()
        {
            boundItem = item;
        }

        public frmBlockManegementAdd()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int __cbGetInt(string item)
        {
            try
            {
                if (item.IndexOf(' ') > 0) item = item.Remove(item.IndexOf(' '));
                return Convert.ToInt32(item);
            }
            catch
            {
                if (parentparent.ranklevels.ContainsKey(item.ToLower())) return parentparent.ranklevels[item.ToLower()];
            }
            return 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cbItem.Text = cbItem.Text.ToLower();
            cbRank.Text = cbRank.Text.ToLower();
            int item = -1;
            try
            {
                item = Convert.ToInt32(cbItem.Text);
            }
            catch
            {
                if(parentparent.blockEnum.ContainsKey(cbItem.Text))
                {
                    item = parentparent.blockEnum[cbItem.Text];
                }
            }
            if (item < 0 || !parentparent.blockIDEnum.ContainsKey(item)) { MessageBox.Show("Invalid item!", "MCAdmin Kit manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            string item_s = item.ToString();
            string rank = __cbGetInt(cbRank.Text).ToString();

            if (boundItem == null)
            {
                foreach (ListViewItem lvi in parent.lvBlocks.Items)
                {
                    if (lvi.SubItems[0].Text == item_s) { MessageBox.Show("Block already in list!", "MCAdmin Block list manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
                }
                parent.lvBlocks.Items.Add(new ListViewItem(new string[] { item_s, parentparent.blockIDEnum[item], rank }));
            }
            else
            {
                boundItem.SubItems[0].Text = item_s;
                boundItem.SubItems[1].Text = parentparent.blockIDEnum[item];
                boundItem.SubItems[2].Text = rank;
            }
            this.Close();
        }

        private void frmBlockManegementAdd_Activated(object sender, EventArgs e)
        {
            cbItem.Focus();
        }

        private void frmBlockManegementAdd_Load(object sender, EventArgs e)
        {
            parentparent = (frmMain)this.Owner.Owner;
            parent = (frmBlocksMangement)this.Owner;
            foreach (string item in parentparent.blockEnum.Keys)
            {
                cbItem.Items.Add(item);
            }
            foreach (KeyValuePair<string, int> kv in parentparent.ranklevels)
            {
                if (kv.Key == "banned") continue;
                cbRank.Items.Add(kv.Value.ToString() + " (" + kv.Key + ")");
                if (kv.Key == parentparent.GetServerProperty("default-rank","guest")) { cbRankDef = cbRank.Items.Count - 1; }
                if (boundItem != null && kv.Value.ToString() == boundItem.SubItems[2].Text) { cbRank.SelectedIndex = cbRank.Items.Count - 1; }
            }
            if (boundItem != null)
            {
                cbItem.Text = boundItem.SubItems[1].Text;
            }
            else
            {
                cbRank.SelectedIndex = cbRankDef; 
            }
        }
    }
}
