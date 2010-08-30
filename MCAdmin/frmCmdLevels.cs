using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCAdmin.Commands;

namespace MCAdmin
{
    public partial class frmCmdLevels : Form
    {
        frmMain parent;

        Dictionary<string, ComboBox> curRanks = new Dictionary<string, ComboBox>();

        public frmCmdLevels()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, ComboBox> kvp in curRanks)
            {
                parent.commands[kvp.Key].minlevel = __cbGetInt(kvp.Value.Text);
            }
            parent.SaveCommandLevels();
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
                if (parent.ranklevels.ContainsKey(item.ToLower())) return parent.ranklevels[item.ToLower()];
            }
            return 0;
        }

        private void frmCmdLevels_Load(object sender, EventArgs e)
        {
            parent = (frmMain)this.Owner;
            int ypos = 0;

            foreach (KeyValuePair<string, Command> kvp in parent.commands)
            {
                Label lblCommand = new Label();
                lblCommand.Text = kvp.Key;
                lblCommand.Parent = panelCommands;
                lblCommand.Location = new Point(0, ypos + 3);
                lblCommand.Size = new Size(50, 25);
                lblCommand.Anchor = AnchorStyles.Left | AnchorStyles.Top;

                ComboBox cbCommandRank = new ComboBox();
                cbCommandRank.Parent = panelCommands;
                cbCommandRank.Location = new Point(60, ypos);
                cbCommandRank.Size = new Size(400, 25);
                cbCommandRank.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (KeyValuePair<string, int> kv in parent.ranklevels)
                {
                    if (kv.Key == "banned") continue;
                    cbCommandRank.Items.Add(kv.Value.ToString() + " (" + kv.Key + ")");
                    if (kv.Value == kvp.Value.minlevel) { cbCommandRank.SelectedIndex = cbCommandRank.Items.Count - 1; }
                }
                cbCommandRank.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                curRanks.Add(kvp.Key, cbCommandRank);

                ypos += 25;
            }
        }
    }
}
