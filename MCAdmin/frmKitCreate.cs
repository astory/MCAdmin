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
    public partial class frmKitCreate : Form
    {
        public frmKitCreate()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ComboBox cbKit = ((frmKits)this.Owner).cbKit;
            Kit newkit = new Kit(this.tbName.Text);
            if (!cbKit.Items.Contains(newkit))
            {
                cbKit.Items.Add(newkit);
                cbKit.SelectedIndex = cbKit.Items.Count - 1;
            }
            this.Close();
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnOK_Click(null, null);
            } 
            else if(e.KeyCode == Keys.Escape) 
            {
                btnCancel_Click(null, null);
            }
        }

        private void frmKitCreate_Activated(object sender, EventArgs e)
        {
            tbName.Focus();
        }
    }
}
