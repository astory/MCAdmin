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
    public partial class frmMasterServerConfig : Form
    {
        public frmMasterServerConfig()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.mbansEnable = cbEnableBans.Checked;
            Program.mbansSubmit = rbBanUseSubmit.Checked;

            Program.mlistEnable = cbEnableList.Checked;
            Program.mlistSendNames = cbListNames.Checked;
            Program.mlistSendRanks = cbListRanks.Checked;

            Program.SaveMasterConfig();

            this.Close();
        }

        private void cbEnableList_CheckedChanged(object sender, EventArgs e)
        {
            cbListNames.Enabled = cbEnableList.Checked;
            cbListNames_CheckedChanged(null, null);
        }

        private void cbEnableBans_CheckedChanged(object sender, EventArgs e)
        {
            rbBanUse.Enabled = cbEnableBans.Checked;
            rbBanUseSubmit.Enabled = cbEnableBans.Checked;
        }

        private void frmMasterServerConfig_Load(object sender, EventArgs e)
        {
            cbEnableBans.Checked = Program.mbansEnable;
            rbBanUseSubmit.Checked = Program.mbansSubmit;

            cbEnableList.Checked = Program.mlistEnable;
            cbListNames.Checked = Program.mlistSendNames;
            cbListRanks.Checked = Program.mlistSendRanks;

            cbEnableBans_CheckedChanged(null, null);
            cbEnableList_CheckedChanged(null, null);
        }

        private void cbListNames_CheckedChanged(object sender, EventArgs e)
        {
            cbListRanks.Enabled = cbListNames.Checked && cbEnableList.Checked;
        }
    }
}
