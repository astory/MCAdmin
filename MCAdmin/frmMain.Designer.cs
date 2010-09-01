namespace MCAdmin
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rtServer = new System.Windows.Forms.RichTextBox();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.btnKillServer = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusDesc = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbManage = new System.Windows.Forms.GroupBox();
            this.btnManageRanks = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnCmdLevels = new System.Windows.Forms.Button();
            this.btnLimitedBlocks = new System.Windows.Forms.Button();
            this.btnIPBans = new System.Windows.Forms.Button();
            this.btnReloadKits = new System.Windows.Forms.Button();
            this.btnManageKits = new System.Windows.Forms.Button();
            this.btnRankConfig = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.tbCommand = new System.Windows.Forms.TextBox();
            this.tmUpdate = new System.Windows.Forms.Timer(this.components);
            this.lbPlayers = new System.Windows.Forms.ListBox();
            this.tmAutosave = new System.Windows.Forms.Timer(this.components);
            this.tmBackup = new System.Windows.Forms.Timer(this.components);
            this.tmUpdateStuff = new System.Windows.Forms.Timer(this.components);
            this.btnRunCmd = new System.Windows.Forms.Button();
            this.gbStatus.SuspendLayout();
            this.gbManage.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtServer
            // 
            this.rtServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtServer.BackColor = System.Drawing.Color.White;
            this.rtServer.ForeColor = System.Drawing.Color.Black;
            this.rtServer.Location = new System.Drawing.Point(12, 92);
            this.rtServer.Name = "rtServer";
            this.rtServer.ReadOnly = true;
            this.rtServer.Size = new System.Drawing.Size(639, 349);
            this.rtServer.TabIndex = 0;
            this.rtServer.Text = "";
            this.rtServer.WordWrap = false;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.btnKillServer);
            this.gbStatus.Controls.Add(this.lblStatus);
            this.gbStatus.Controls.Add(this.lblStatusDesc);
            this.gbStatus.Controls.Add(this.btnRestart);
            this.gbStatus.Controls.Add(this.btnStop);
            this.gbStatus.Controls.Add(this.btnStart);
            this.gbStatus.Location = new System.Drawing.Point(12, 12);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(245, 74);
            this.gbStatus.TabIndex = 1;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Server status";
            // 
            // btnKillServer
            // 
            this.btnKillServer.Enabled = false;
            this.btnKillServer.Location = new System.Drawing.Point(164, 12);
            this.btnKillServer.Name = "btnKillServer";
            this.btnKillServer.Size = new System.Drawing.Size(73, 25);
            this.btnKillServer.TabIndex = 5;
            this.btnKillServer.Text = "Kill";
            this.btnKillServer.UseVisualStyleBackColor = true;
            this.btnKillServer.Click += new System.EventHandler(this.btnKillServer_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(46, 22);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Stopped";
            // 
            // lblStatusDesc
            // 
            this.lblStatusDesc.AutoSize = true;
            this.lblStatusDesc.Location = new System.Drawing.Point(6, 22);
            this.lblStatusDesc.Name = "lblStatusDesc";
            this.lblStatusDesc.Size = new System.Drawing.Size(40, 13);
            this.lblStatusDesc.TabIndex = 3;
            this.lblStatusDesc.Text = "Status:";
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(164, 43);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(73, 25);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(85, 43);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(73, 25);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(9, 43);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(73, 25);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbManage
            // 
            this.gbManage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbManage.Controls.Add(this.btnManageRanks);
            this.gbManage.Controls.Add(this.btnSave);
            this.gbManage.Controls.Add(this.btnBackup);
            this.gbManage.Controls.Add(this.btnCmdLevels);
            this.gbManage.Controls.Add(this.btnLimitedBlocks);
            this.gbManage.Controls.Add(this.btnIPBans);
            this.gbManage.Controls.Add(this.btnReloadKits);
            this.gbManage.Controls.Add(this.btnManageKits);
            this.gbManage.Controls.Add(this.btnRankConfig);
            this.gbManage.Controls.Add(this.btnProperties);
            this.gbManage.Location = new System.Drawing.Point(263, 12);
            this.gbManage.Name = "gbManage";
            this.gbManage.Size = new System.Drawing.Size(566, 74);
            this.gbManage.TabIndex = 2;
            this.gbManage.TabStop = false;
            this.gbManage.Text = "Server management";
            // 
            // btnManageRanks
            // 
            this.btnManageRanks.Location = new System.Drawing.Point(98, 43);
            this.btnManageRanks.Name = "btnManageRanks";
            this.btnManageRanks.Size = new System.Drawing.Size(86, 25);
            this.btnManageRanks.TabIndex = 9;
            this.btnManageRanks.Text = "Rank config";
            this.btnManageRanks.UseVisualStyleBackColor = true;
            this.btnManageRanks.Click += new System.EventHandler(this.btnManageRanks_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(282, 43);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 25);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save NOW";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.tmAutosave_Tick);
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(282, 19);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(86, 25);
            this.btnBackup.TabIndex = 7;
            this.btnBackup.Text = "Backup NOW";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnCmdLevels
            // 
            this.btnCmdLevels.Location = new System.Drawing.Point(466, 19);
            this.btnCmdLevels.Name = "btnCmdLevels";
            this.btnCmdLevels.Size = new System.Drawing.Size(86, 49);
            this.btnCmdLevels.TabIndex = 6;
            this.btnCmdLevels.Text = "Command levels";
            this.btnCmdLevels.UseVisualStyleBackColor = true;
            this.btnCmdLevels.Click += new System.EventHandler(this.btnCmdLevels_Click);
            // 
            // btnLimitedBlocks
            // 
            this.btnLimitedBlocks.Location = new System.Drawing.Point(374, 19);
            this.btnLimitedBlocks.Name = "btnLimitedBlocks";
            this.btnLimitedBlocks.Size = new System.Drawing.Size(86, 49);
            this.btnLimitedBlocks.TabIndex = 5;
            this.btnLimitedBlocks.Text = "Limited blocks";
            this.btnLimitedBlocks.UseVisualStyleBackColor = true;
            this.btnLimitedBlocks.Click += new System.EventHandler(this.btnLimitedBlocks_Click);
            // 
            // btnIPBans
            // 
            this.btnIPBans.Location = new System.Drawing.Point(6, 43);
            this.btnIPBans.Name = "btnIPBans";
            this.btnIPBans.Size = new System.Drawing.Size(86, 25);
            this.btnIPBans.TabIndex = 4;
            this.btnIPBans.Text = "IP bans";
            this.btnIPBans.UseVisualStyleBackColor = true;
            this.btnIPBans.Click += new System.EventHandler(this.btnIPBans_Click);
            // 
            // btnReloadKits
            // 
            this.btnReloadKits.Location = new System.Drawing.Point(190, 43);
            this.btnReloadKits.Name = "btnReloadKits";
            this.btnReloadKits.Size = new System.Drawing.Size(86, 25);
            this.btnReloadKits.TabIndex = 3;
            this.btnReloadKits.Text = "Reload kits";
            this.btnReloadKits.UseVisualStyleBackColor = true;
            this.btnReloadKits.Click += new System.EventHandler(this.btnReloadKits_Click);
            // 
            // btnManageKits
            // 
            this.btnManageKits.Location = new System.Drawing.Point(190, 19);
            this.btnManageKits.Name = "btnManageKits";
            this.btnManageKits.Size = new System.Drawing.Size(86, 25);
            this.btnManageKits.TabIndex = 2;
            this.btnManageKits.Text = "Edit kits";
            this.btnManageKits.UseVisualStyleBackColor = true;
            this.btnManageKits.Click += new System.EventHandler(this.btnManageKits_Click);
            // 
            // btnRankConfig
            // 
            this.btnRankConfig.Location = new System.Drawing.Point(98, 19);
            this.btnRankConfig.Name = "btnRankConfig";
            this.btnRankConfig.Size = new System.Drawing.Size(86, 25);
            this.btnRankConfig.TabIndex = 1;
            this.btnRankConfig.Text = "Player ranks";
            this.btnRankConfig.UseVisualStyleBackColor = true;
            this.btnRankConfig.Click += new System.EventHandler(this.btnRankConfig_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(6, 19);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(86, 25);
            this.btnProperties.TabIndex = 0;
            this.btnProperties.Text = "Server props.";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // tbCommand
            // 
            this.tbCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCommand.Location = new System.Drawing.Point(12, 450);
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.Size = new System.Drawing.Size(725, 20);
            this.tbCommand.TabIndex = 3;
            this.tbCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCommand_KeyDown);
            // 
            // tmUpdate
            // 
            this.tmUpdate.Enabled = true;
            this.tmUpdate.Interval = 3600000;
            this.tmUpdate.Tick += new System.EventHandler(this.tmUpdate_Tick);
            // 
            // lbPlayers
            // 
            this.lbPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlayers.FormattingEnabled = true;
            this.lbPlayers.IntegralHeight = false;
            this.lbPlayers.Location = new System.Drawing.Point(657, 92);
            this.lbPlayers.Name = "lbPlayers";
            this.lbPlayers.Size = new System.Drawing.Size(172, 349);
            this.lbPlayers.TabIndex = 4;
            // 
            // tmAutosave
            // 
            this.tmAutosave.Tick += new System.EventHandler(this.tmAutosave_Tick);
            // 
            // tmBackup
            // 
            this.tmBackup.Tick += new System.EventHandler(this.tmBackup_Tick);
            // 
            // tmUpdateStuff
            // 
            this.tmUpdateStuff.Enabled = true;
            this.tmUpdateStuff.Interval = 1000;
            this.tmUpdateStuff.Tick += new System.EventHandler(this.tmUpdateStuff_Tick);
            // 
            // btnRunCmd
            // 
            this.btnRunCmd.Location = new System.Drawing.Point(743, 447);
            this.btnRunCmd.Name = "btnRunCmd";
            this.btnRunCmd.Size = new System.Drawing.Size(86, 25);
            this.btnRunCmd.TabIndex = 10;
            this.btnRunCmd.Text = "Run";
            this.btnRunCmd.UseVisualStyleBackColor = true;
            this.btnRunCmd.Click += new System.EventHandler(this.btnRunCmd_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 479);
            this.Controls.Add(this.btnRunCmd);
            this.Controls.Add(this.lbPlayers);
            this.Controls.Add(this.tbCommand);
            this.Controls.Add(this.gbManage);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.rtServer);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCAdmin (c) by Doridian 2010";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbManage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtServer;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Label lblStatusDesc;
        private System.Windows.Forms.GroupBox gbManage;
        private System.Windows.Forms.TextBox tbCommand;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnRankConfig;
        private System.Windows.Forms.Button btnManageKits;
        private System.Windows.Forms.Button btnReloadKits;
        private System.Windows.Forms.Timer tmUpdate;
        private System.Windows.Forms.Button btnIPBans;
        private System.Windows.Forms.Button btnLimitedBlocks;
        private System.Windows.Forms.Timer tmAutosave;
        private System.Windows.Forms.Button btnCmdLevels;
        private System.Windows.Forms.Timer tmBackup;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.ListBox lbPlayers;
        private System.Windows.Forms.Timer tmUpdateStuff;
        public System.Windows.Forms.Button btnRestart;
        public System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.Button btnKillServer;
        public System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnManageRanks;
        private System.Windows.Forms.Button btnRunCmd;
    }
}

