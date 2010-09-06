using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MCAdmin
{
    public partial class frmMain : Form
    {
        private delegate void AddRTLineDelegate(Color color, string line);
        private AddRTLineDelegate AddRTLineInt;

        public delegate void SetStartEnabledDelegate(bool enabled);
        public SetStartEnabledDelegate SetStartEnabledInt;

        public frmMain()
        {
            InitializeComponent();
        }        

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            AddRTLineInt = new AddRTLineDelegate(AddRTLineMethod);
            SetStartEnabledInt = new SetStartEnabledDelegate(SetStartEnabledMethod);

            if (!File.Exists("minecraft_server.jar")) btnStart.Enabled = false;

            Program.frmMainReady = true;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (File.Exists("MCAdmin.exe.new")) File.Delete("MCAdmin.exe.new");
            }
            catch { }
            try
            {
                if (File.Exists("minecraft_server.jar.new")) File.Delete("minecraft_server.jar.new");
            }
            catch { }
            try
            {
                Program.updaterThread.Abort();
            }
            catch { }
            Program.KillServer();
            try
            {
                Program.serverRcon.Dispose();
            }
            catch { }
            try
            {
                Program.serverQuery.Dispose();
            }
            catch { }
        }

        public void AddRTLine(Color color, string line)
        {
            try
            {
                if (rtServer.InvokeRequired)
                    rtServer.BeginInvoke(AddRTLineInt, new object[] { color, line });
                else
                    AddRTLineMethod(color, line);
            }
            catch { }
        }

        #region Delegate internal methods
        private void AddRTLineMethod(Color color, string line)
        {
            rtServer.SelectionColor = color;
            rtServer.SelectionStart = rtServer.TextLength;
            rtServer.SelectedText = line;
            rtServer.DeselectAll();
            rtServer.SelectionStart = rtServer.TextLength;
            rtServer.ScrollToCaret();
        }

        private void SetStartEnabledMethod(bool enabled)
        {
            btnStart.Enabled = enabled;
        }
        #endregion

        #region Server status controls
        private void btnKillServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to KILL the server?\r\nThis means the level will not be saved!", "MCAdmin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                Program.KillServer();
        }

        public void btnStart_Click(object sender, System.EventArgs e)
        {
            Program.StartServer();
        }

        public void btnStop_Click(object sender, System.EventArgs e)
        {
            Program.StopServer();
        }

        public void btnRestart_Click(object sender, EventArgs e)
        {
            Program.AddRTLine(Color.Black, "Server restarting!\r\n",true);
            Program.StopServer();
            Program.StartServer();
        }
        #endregion

        private void tbCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnRunCmd_Click(null, null);
            }
        }

        private void btnProperties_Click(object sender, System.EventArgs e)
        {
            new frmProperties().ShowDialog(this);
        }

        private void btnRankConfig_Click(object sender, System.EventArgs e)
        {
            new frmRanks().ShowDialog(this);
        }

        private void btnManageKits_Click(object sender, EventArgs e)
        {
            new frmKits().ShowDialog(this);
        }

        private void btnReloadKits_Click(object sender, EventArgs e)
        {
            Program.SaveKits(false);
            Program.LoadKits();
        }

        private void btnIPBans_Click(object sender, EventArgs e)
        {
            new frmIPBans().ShowDialog(this);
        }

        private void btnLimitedBlocks_Click(object sender, EventArgs e)
        {
            new frmBlocksMangement().ShowDialog(this);
        }

        private void btnCmdLevels_Click(object sender, EventArgs e)
        {
            new frmCmdLevels().ShowDialog(this);
        }

        private void btnManageRanks_Click(object sender, EventArgs e)
        {
            new frmRankConfig().ShowDialog(this);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            Program.tmBackup_Tick(null,null);
        }

        private void btnRunCmd_Click(object sender, EventArgs e)
        {
            Program.SendServerCommand(tbCommand.Text);
            tbCommand.Text = "";
        }

        private void tmUpdateStuff_Tick(object sender, EventArgs e)
        {
            try
            {
                lbPlayers.Items.Clear();
                if (Program.minecraftServer == null || Program.minecraftServer.HasExited || !Program.serverFullyOnline) return;
                foreach (Player ply in Program.minecraftFirewall.players)
                {
                    if (ply.name == null || ply.name == "") continue;
                    lbPlayers.Items.Add(ply.name);
                }
            }
            catch { }
        }
    }
}