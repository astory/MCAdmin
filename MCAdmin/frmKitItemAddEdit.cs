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
    public partial class frmKitItemAddEdit : Form
    {
        frmMain parentparent;
        frmKits parent;
        ListViewItem boundItem;

        public frmKitItemAddEdit(ListViewItem item)
            : this()
        {
            boundItem = item;
        }

        public frmKitItemAddEdit()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cbItem.Text = cbItem.Text.ToLower();
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
            if (boundItem == null)
            {
                foreach (ListViewItem lvi in parent.lvItems.Items)
                {
                    if (lvi.SubItems[0].Text == item_s) { MessageBox.Show("Item already in kit!", "MCAdmin Kit manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
                }
                parent.lvItems.Items.Add(new ListViewItem(new string[] { item_s, parentparent.blockIDEnum[item], numAmount.Value.ToString() }));
            }
            else
            {
                boundItem.SubItems[0].Text = item_s;
                boundItem.SubItems[1].Text = parentparent.blockIDEnum[item];
                boundItem.SubItems[2].Text = numAmount.Value.ToString();
            }
            this.Close();
        }

        private void frmKitCreate_Activated(object sender, EventArgs e)
        {
            cbItem.Focus();
        }

        private void frmKitItemAddEdit_Load(object sender, EventArgs e)
        {
            parentparent = (frmMain)this.Owner.Owner;
            parent = (frmKits)this.Owner;
            foreach (string item in parentparent.blockEnum.Keys)
            {
                cbItem.Items.Add(item);
            }
            if (boundItem != null)
            {
                cbItem.Text = boundItem.SubItems[1].Text;
                numAmount.Value = Convert.ToInt32(boundItem.SubItems[2].Text);
            }
        }
    }
}
