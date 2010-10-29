namespace MCAdmin
{
    partial class frmMasterServerConfig
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
            this.cbEnableList = new System.Windows.Forms.CheckBox();
            this.cbEnableBans = new System.Windows.Forms.CheckBox();
            this.cbListNames = new System.Windows.Forms.CheckBox();
            this.cbListRanks = new System.Windows.Forms.CheckBox();
            this.rbBanUseSubmit = new System.Windows.Forms.RadioButton();
            this.rbBanUse = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbEnableList
            // 
            this.cbEnableList.AutoSize = true;
            this.cbEnableList.Checked = true;
            this.cbEnableList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableList.Location = new System.Drawing.Point(12, 12);
            this.cbEnableList.Name = "cbEnableList";
            this.cbEnableList.Size = new System.Drawing.Size(172, 17);
            this.cbEnableList.TabIndex = 0;
            this.cbEnableList.Text = "List this server on the masterlist";
            this.cbEnableList.UseVisualStyleBackColor = true;
            this.cbEnableList.CheckedChanged += new System.EventHandler(this.cbEnableList_CheckedChanged);
            // 
            // cbEnableBans
            // 
            this.cbEnableBans.AutoSize = true;
            this.cbEnableBans.Checked = true;
            this.cbEnableBans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableBans.Location = new System.Drawing.Point(12, 81);
            this.cbEnableBans.Name = "cbEnableBans";
            this.cbEnableBans.Size = new System.Drawing.Size(123, 17);
            this.cbEnableBans.TabIndex = 1;
            this.cbEnableBans.Text = "Enable global banlist";
            this.cbEnableBans.UseVisualStyleBackColor = true;
            this.cbEnableBans.CheckedChanged += new System.EventHandler(this.cbEnableBans_CheckedChanged);
            // 
            // cbListNames
            // 
            this.cbListNames.AutoSize = true;
            this.cbListNames.Checked = true;
            this.cbListNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbListNames.Location = new System.Drawing.Point(55, 35);
            this.cbListNames.Name = "cbListNames";
            this.cbListNames.Size = new System.Drawing.Size(116, 17);
            this.cbListNames.TabIndex = 3;
            this.cbListNames.Text = "Send player names";
            this.cbListNames.UseVisualStyleBackColor = true;
            this.cbListNames.CheckedChanged += new System.EventHandler(this.cbListNames_CheckedChanged);
            // 
            // cbListRanks
            // 
            this.cbListRanks.AutoSize = true;
            this.cbListRanks.Checked = true;
            this.cbListRanks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbListRanks.Location = new System.Drawing.Point(55, 58);
            this.cbListRanks.Name = "cbListRanks";
            this.cbListRanks.Size = new System.Drawing.Size(118, 17);
            this.cbListRanks.TabIndex = 4;
            this.cbListRanks.Text = "Send player\'s ranks";
            this.cbListRanks.UseVisualStyleBackColor = true;
            // 
            // rbBanUseSubmit
            // 
            this.rbBanUseSubmit.AutoSize = true;
            this.rbBanUseSubmit.Checked = true;
            this.rbBanUseSubmit.Location = new System.Drawing.Point(50, 104);
            this.rbBanUseSubmit.Name = "rbBanUseSubmit";
            this.rbBanUseSubmit.Size = new System.Drawing.Size(289, 17);
            this.rbBanUseSubmit.TabIndex = 5;
            this.rbBanUseSubmit.TabStop = true;
            this.rbBanUseSubmit.Text = "Use global bans and submit server bans to global banlist";
            this.rbBanUseSubmit.UseVisualStyleBackColor = true;
            // 
            // rbBanUse
            // 
            this.rbBanUse.AutoSize = true;
            this.rbBanUse.Location = new System.Drawing.Point(50, 127);
            this.rbBanUse.Name = "rbBanUse";
            this.rbBanUse.Size = new System.Drawing.Size(310, 17);
            this.rbBanUse.TabIndex = 6;
            this.rbBanUse.Text = "Only use bans from the global banlist, but do not submit bans";
            this.rbBanUse.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 153);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Size = new System.Drawing.Size(356, 35);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 27);
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
            this.btnSave.Size = new System.Drawing.Size(169, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmMasterServerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 200);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.rbBanUse);
            this.Controls.Add(this.rbBanUseSubmit);
            this.Controls.Add(this.cbListRanks);
            this.Controls.Add(this.cbListNames);
            this.Controls.Add(this.cbEnableBans);
            this.Controls.Add(this.cbEnableList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmMasterServerConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Master server config";
            this.Load += new System.EventHandler(this.frmMasterServerConfig_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbEnableList;
        private System.Windows.Forms.CheckBox cbEnableBans;
        private System.Windows.Forms.CheckBox cbListNames;
        private System.Windows.Forms.CheckBox cbListRanks;
        private System.Windows.Forms.RadioButton rbBanUseSubmit;
        private System.Windows.Forms.RadioButton rbBanUse;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}