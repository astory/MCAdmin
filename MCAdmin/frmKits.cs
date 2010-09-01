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
    public partial class frmKits : Form
    {
        frmMain parent;
        bool extended = true;
        int cbReqLevelDef;

        public frmKits()
        {
            InitializeComponent();
        }

        private void btnNewKit_Click(object sender, EventArgs e)
        {
            new frmKitCreate().ShowDialog(this);
        }

        private void frmKits_Load(object sender, EventArgs e)
        {
            SetExtended(false);
            parent = (frmMain)this.Owner;
            foreach (KeyValuePair<string, int> kv in parent.ranklevels)
            {
                if (kv.Key == "banned") continue;
                cbReqLevel.Items.Add(kv.Value.ToString() + " (" + kv.Key + ")");
                if (kv.Key == parent.GetServerProperty("default-rank", "guest")) { cbReqLevelDef = cbReqLevel.Items.Count - 1; cbReqLevel.SelectedIndex = cbReqLevelDef; }
            }
            foreach (Kit k in parent.kits)
            {
                cbKit.Items.Add(k);
            }
        }

        public void SetExtended(bool newextended)
        {
            if (newextended == extended) return;

            extended = newextended;

            if (extended)
            {
                this.Size = new Size(this.Size.Width, 490);
            }
            else
            {
                this.Size = new Size(this.Size.Width, 75);
            }

            lblItems.Visible = extended;
            lblReqLevel.Visible = extended;
            lvItems.Visible = extended;
            cbReqLevel.Visible = extended;
            splitContainer1.Visible = extended;
            btnAddItem.Visible = extended;
            btnDelItem.Visible = extended;
            //btnImport.Visible = extended;
        }

        private int __cbGetInt(string item)
        {
            try
            {
                if(item.IndexOf(' ') > 0) item = item.Remove(item.IndexOf(' '));
                return Convert.ToInt32(item);
            }
            catch
            {
                if (parent.ranklevels.ContainsKey(item.ToLower())) return parent.ranklevels[item.ToLower()];
            }
            return 0;
        }

        private void cbKit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbKit.SelectedIndex < 0) { SetExtended(false); return; }
            Kit kit = (Kit)cbKit.SelectedItem;
            cbReqLevel.SelectedIndex = cbReqLevelDef;
            lvItems.Items.Clear();
            for (int i = 0; i < cbReqLevel.Items.Count; i++)
            {
                if (__cbGetInt(cbReqLevel.Items[i].ToString()) == kit.reqlevel) { cbReqLevel.SelectedIndex = i; break; }
            }
            foreach(KeyValuePair<int,int> kv in kit.items)
            {
                lvItems.Items.Add(new ListViewItem(new string[] { kv.Key.ToString(), parent.blockIDEnum[kv.Key], kv.Value.ToString() }));
            }
            SetExtended(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbKit.SelectedIndex < 0) return;
            Kit kit = (Kit)cbKit.SelectedItem;
            cbKit.Items.RemoveAt(cbKit.SelectedIndex);
            if (parent.kits.Contains(kit))
            {
                parent.kits.Remove(kit);
                parent.SaveKits();
            }
            SetExtended(false);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            new frmKitItemAddEdit().ShowDialog(this);
        }

        private void btnDelItem_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count <= 0) return;
            lvItems.SelectedItems[0].Remove();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Kit kit = (Kit)cbKit.SelectedItem;
            kit.reqlevel = __cbGetInt(cbReqLevel.SelectedItem.ToString());
            kit.items.Clear();

            foreach (ListViewItem lvi in lvItems.Items)
            {
                kit.items.Add(Convert.ToInt32(lvi.SubItems[0].Text), Convert.ToInt32(lvi.SubItems[2].Text));
            }

            if (parent.kits.Contains(kit)) parent.kits.Remove(kit);
            parent.kits.Add(kit);
            parent.SaveKits();
            SetExtended(false);
            cbKit.Text = "";
            cbKit.SelectedItem = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetExtended(false);
            Kit kit = (Kit)cbKit.SelectedItem;
            if (!kit.saved) cbKit.Items.Remove(kit);
            cbKit.Text = "";
            cbKit.SelectedItem = null;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count <= 0) return;
            new frmKitItemAddEdit(lvItems.SelectedItems[0]).ShowDialog(this);
        }

        private void btnKitShare_Click(object sender, EventArgs e)
        {
            new frmKitShare().ShowDialog(parent);
        }
    }
}
