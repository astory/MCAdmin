namespace MCAdmin
{
    partial class frmProperties
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
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbLevel = new System.Windows.Forms.TextBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.tbPreview = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ttBase = new System.Windows.Forms.ToolTip(this.components);
            this.cbIP = new System.Windows.Forms.ComboBox();
            this.lblIntPort = new System.Windows.Forms.Label();
            this.tbIntPort = new System.Windows.Forms.TextBox();
            this.lblXmxXms = new System.Windows.Forms.Label();
            this.numMemory = new System.Windows.Forms.NumericUpDown();
            this.lblRecMem = new System.Windows.Forms.Label();
            this.cbOnline = new System.Windows.Forms.CheckBox();
            this.lblDefRank = new System.Windows.Forms.Label();
            this.cbDefRank = new System.Windows.Forms.ComboBox();
            this.lblASPrefix = new System.Windows.Forms.Label();
            this.numAS = new System.Windows.Forms.NumericUpDown();
            this.lblASSuffix = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numBackup = new System.Windows.Forms.NumericUpDown();
            this.lblBackup = new System.Windows.Forms.Label();
            this.cbRCONEnable = new System.Windows.Forms.CheckBox();
            this.tbRCONPort = new System.Windows.Forms.TextBox();
            this.lblRCONPort = new System.Windows.Forms.Label();
            this.tbRCONPass = new System.Windows.Forms.TextBox();
            this.lblRCONPass = new System.Windows.Forms.Label();
            this.btnAutoDetect = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBackup)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(9, 9);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(9, 35);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // tbPort
            // 
            this.tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPort.Location = new System.Drawing.Point(86, 32);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(172, 20);
            this.tbPort.TabIndex = 2;
            this.tbPort.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            this.tbPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericStuff_KeyPress);
            // 
            // tbLevel
            // 
            this.tbLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLevel.Location = new System.Drawing.Point(86, 58);
            this.tbLevel.Name = "tbLevel";
            this.tbLevel.Size = new System.Drawing.Size(361, 20);
            this.tbLevel.TabIndex = 4;
            this.tbLevel.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(9, 61);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(65, 13);
            this.lblLevel.TabIndex = 5;
            this.lblLevel.Text = "Level name:";
            // 
            // tbPreview
            // 
            this.tbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPreview.BackColor = System.Drawing.Color.White;
            this.tbPreview.Location = new System.Drawing.Point(9, 237);
            this.tbPreview.Multiline = true;
            this.tbPreview.Name = "tbPreview";
            this.tbPreview.ReadOnly = true;
            this.tbPreview.Size = new System.Drawing.Size(435, 206);
            this.tbPreview.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 449);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Size = new System.Drawing.Size(435, 35);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(209, 27);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(5, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(208, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ttBase
            // 
            this.ttBase.IsBalloon = true;
            // 
            // cbIP
            // 
            this.cbIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIP.FormattingEnabled = true;
            this.cbIP.Location = new System.Drawing.Point(86, 5);
            this.cbIP.Name = "cbIP";
            this.cbIP.Size = new System.Drawing.Size(277, 21);
            this.cbIP.TabIndex = 10;
            this.cbIP.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblIntPort
            // 
            this.lblIntPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIntPort.AutoSize = true;
            this.lblIntPort.Location = new System.Drawing.Point(264, 35);
            this.lblIntPort.Name = "lblIntPort";
            this.lblIntPort.Size = new System.Drawing.Size(47, 13);
            this.lblIntPort.TabIndex = 11;
            this.lblIntPort.Text = "Int. Port:";
            // 
            // tbIntPort
            // 
            this.tbIntPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIntPort.Location = new System.Drawing.Point(317, 32);
            this.tbIntPort.Name = "tbIntPort";
            this.tbIntPort.Size = new System.Drawing.Size(130, 20);
            this.tbIntPort.TabIndex = 12;
            this.tbIntPort.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            this.tbIntPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericStuff_KeyPress);
            // 
            // lblXmxXms
            // 
            this.lblXmxXms.AutoSize = true;
            this.lblXmxXms.Location = new System.Drawing.Point(9, 113);
            this.lblXmxXms.Name = "lblXmxXms";
            this.lblXmxXms.Size = new System.Drawing.Size(128, 13);
            this.lblXmxXms.TabIndex = 13;
            this.lblXmxXms.Text = "Assigned memory (in MB):";
            // 
            // numMemory
            // 
            this.numMemory.Location = new System.Drawing.Point(143, 111);
            this.numMemory.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numMemory.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.numMemory.Name = "numMemory";
            this.numMemory.Size = new System.Drawing.Size(90, 20);
            this.numMemory.TabIndex = 14;
            this.numMemory.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numMemory.ValueChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblRecMem
            // 
            this.lblRecMem.AutoSize = true;
            this.lblRecMem.Location = new System.Drawing.Point(239, 113);
            this.lblRecMem.Name = "lblRecMem";
            this.lblRecMem.Size = new System.Drawing.Size(153, 13);
            this.lblRecMem.TabIndex = 15;
            this.lblRecMem.Text = "(Recommended: min. 1024MB)";
            // 
            // cbOnline
            // 
            this.cbOnline.AutoSize = true;
            this.cbOnline.Location = new System.Drawing.Point(9, 189);
            this.cbOnline.Name = "cbOnline";
            this.cbOnline.Size = new System.Drawing.Size(158, 17);
            this.cbOnline.TabIndex = 16;
            this.cbOnline.Text = "Run in online (secure) mode";
            this.cbOnline.UseVisualStyleBackColor = true;
            this.cbOnline.CheckedChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblDefRank
            // 
            this.lblDefRank.AutoSize = true;
            this.lblDefRank.Location = new System.Drawing.Point(9, 87);
            this.lblDefRank.Name = "lblDefRank";
            this.lblDefRank.Size = new System.Drawing.Size(68, 13);
            this.lblDefRank.TabIndex = 17;
            this.lblDefRank.Text = "Default rank:";
            // 
            // cbDefRank
            // 
            this.cbDefRank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDefRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefRank.FormattingEnabled = true;
            this.cbDefRank.Location = new System.Drawing.Point(86, 84);
            this.cbDefRank.Name = "cbDefRank";
            this.cbDefRank.Size = new System.Drawing.Size(361, 21);
            this.cbDefRank.TabIndex = 18;
            this.cbDefRank.SelectedIndexChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblASPrefix
            // 
            this.lblASPrefix.AutoSize = true;
            this.lblASPrefix.Location = new System.Drawing.Point(9, 139);
            this.lblASPrefix.Name = "lblASPrefix";
            this.lblASPrefix.Size = new System.Drawing.Size(81, 13);
            this.lblASPrefix.TabIndex = 19;
            this.lblASPrefix.Text = "Autosave every";
            // 
            // numAS
            // 
            this.numAS.Location = new System.Drawing.Point(96, 137);
            this.numAS.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numAS.Name = "numAS";
            this.numAS.Size = new System.Drawing.Size(87, 20);
            this.numAS.TabIndex = 20;
            this.numAS.ValueChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblASSuffix
            // 
            this.lblASSuffix.AutoSize = true;
            this.lblASSuffix.Location = new System.Drawing.Point(189, 139);
            this.lblASSuffix.Name = "lblASSuffix";
            this.lblASSuffix.Size = new System.Drawing.Size(109, 13);
            this.lblASSuffix.TabIndex = 21;
            this.lblASSuffix.Text = "minutes (0 to disable).";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "minutes (0 to disable).";
            // 
            // numBackup
            // 
            this.numBackup.Location = new System.Drawing.Point(96, 163);
            this.numBackup.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numBackup.Name = "numBackup";
            this.numBackup.Size = new System.Drawing.Size(87, 20);
            this.numBackup.TabIndex = 23;
            this.numBackup.ValueChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblBackup
            // 
            this.lblBackup.AutoSize = true;
            this.lblBackup.Location = new System.Drawing.Point(9, 165);
            this.lblBackup.Name = "lblBackup";
            this.lblBackup.Size = new System.Drawing.Size(73, 13);
            this.lblBackup.TabIndex = 22;
            this.lblBackup.Text = "Backup every";
            // 
            // cbRCONEnable
            // 
            this.cbRCONEnable.AutoSize = true;
            this.cbRCONEnable.Location = new System.Drawing.Point(9, 214);
            this.cbRCONEnable.Name = "cbRCONEnable";
            this.cbRCONEnable.Size = new System.Drawing.Size(131, 17);
            this.cbRCONEnable.TabIndex = 25;
            this.cbRCONEnable.Text = "Enable source RCON:";
            this.cbRCONEnable.UseVisualStyleBackColor = true;
            this.cbRCONEnable.CheckedChanged += new System.EventHandler(this.cbRCONEnable_CheckedChanged);
            // 
            // tbRCONPort
            // 
            this.tbRCONPort.Location = new System.Drawing.Point(181, 211);
            this.tbRCONPort.Name = "tbRCONPort";
            this.tbRCONPort.Size = new System.Drawing.Size(80, 20);
            this.tbRCONPort.TabIndex = 26;
            this.tbRCONPort.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            this.tbRCONPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericStuff_KeyPress);
            // 
            // lblRCONPort
            // 
            this.lblRCONPort.AutoSize = true;
            this.lblRCONPort.Location = new System.Drawing.Point(146, 215);
            this.lblRCONPort.Name = "lblRCONPort";
            this.lblRCONPort.Size = new System.Drawing.Size(29, 13);
            this.lblRCONPort.TabIndex = 27;
            this.lblRCONPort.Text = "Port:";
            // 
            // tbRCONPass
            // 
            this.tbRCONPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRCONPass.Location = new System.Drawing.Point(306, 211);
            this.tbRCONPass.Name = "tbRCONPass";
            this.tbRCONPass.Size = new System.Drawing.Size(138, 20);
            this.tbRCONPass.TabIndex = 28;
            this.tbRCONPass.TextChanged += new System.EventHandler(this.event_RefreshPreview);
            // 
            // lblRCONPass
            // 
            this.lblRCONPass.AutoSize = true;
            this.lblRCONPass.Location = new System.Drawing.Point(267, 214);
            this.lblRCONPass.Name = "lblRCONPass";
            this.lblRCONPass.Size = new System.Drawing.Size(33, 13);
            this.lblRCONPass.TabIndex = 29;
            this.lblRCONPass.Text = "Pass:";
            // 
            // btnAutoDetect
            // 
            this.btnAutoDetect.Location = new System.Drawing.Point(369, 5);
            this.btnAutoDetect.Name = "btnAutoDetect";
            this.btnAutoDetect.Size = new System.Drawing.Size(78, 21);
            this.btnAutoDetect.TabIndex = 30;
            this.btnAutoDetect.Text = "Auto-detect";
            this.btnAutoDetect.UseVisualStyleBackColor = true;
            this.btnAutoDetect.Click += new System.EventHandler(this.btnAutoDetect_Click);
            // 
            // frmProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 496);
            this.Controls.Add(this.btnAutoDetect);
            this.Controls.Add(this.lblRCONPass);
            this.Controls.Add(this.tbRCONPass);
            this.Controls.Add(this.lblRCONPort);
            this.Controls.Add(this.tbRCONPort);
            this.Controls.Add(this.cbRCONEnable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numBackup);
            this.Controls.Add(this.lblBackup);
            this.Controls.Add(this.lblASSuffix);
            this.Controls.Add(this.numAS);
            this.Controls.Add(this.lblASPrefix);
            this.Controls.Add(this.cbDefRank);
            this.Controls.Add(this.lblDefRank);
            this.Controls.Add(this.cbOnline);
            this.Controls.Add(this.lblRecMem);
            this.Controls.Add(this.numMemory);
            this.Controls.Add(this.lblXmxXms);
            this.Controls.Add(this.tbIntPort);
            this.Controls.Add(this.lblIntPort);
            this.Controls.Add(this.cbIP);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tbPreview);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.tbLevel);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server properties";
            this.Load += new System.EventHandler(this.frmProperties_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBackup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbLevel;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.TextBox tbPreview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip ttBase;
        private System.Windows.Forms.ComboBox cbIP;
        private System.Windows.Forms.Label lblIntPort;
        private System.Windows.Forms.TextBox tbIntPort;
        private System.Windows.Forms.Label lblXmxXms;
        private System.Windows.Forms.NumericUpDown numMemory;
        private System.Windows.Forms.Label lblRecMem;
        private System.Windows.Forms.CheckBox cbOnline;
        private System.Windows.Forms.Label lblDefRank;
        private System.Windows.Forms.ComboBox cbDefRank;
        private System.Windows.Forms.Label lblASPrefix;
        private System.Windows.Forms.NumericUpDown numAS;
        private System.Windows.Forms.Label lblASSuffix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numBackup;
        private System.Windows.Forms.Label lblBackup;
        private System.Windows.Forms.CheckBox cbRCONEnable;
        private System.Windows.Forms.TextBox tbRCONPort;
        private System.Windows.Forms.Label lblRCONPort;
        private System.Windows.Forms.TextBox tbRCONPass;
        private System.Windows.Forms.Label lblRCONPass;
        private System.Windows.Forms.Button btnAutoDetect;
    }
}