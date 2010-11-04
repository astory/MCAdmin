namespace MCAdmin
{
    partial class frmIPBans
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
            this.lbIPBans = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbBanIP = new System.Windows.Forms.TextBox();
            this.btnAddBan = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbIPBans
            // 
            this.lbIPBans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbIPBans.FormattingEnabled = true;
            this.lbIPBans.IntegralHeight = false;
            this.lbIPBans.Location = new System.Drawing.Point(12, 12);
            this.lbIPBans.Name = "lbIPBans";
            this.lbIPBans.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbIPBans.Size = new System.Drawing.Size(439, 298);
            this.lbIPBans.TabIndex = 0;
            this.lbIPBans.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbIPBans_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 342);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Size = new System.Drawing.Size(439, 35);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(211, 27);
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
            this.btnSave.Size = new System.Drawing.Size(210, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbBanIP
            // 
            this.tbBanIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBanIP.Location = new System.Drawing.Point(15, 316);
            this.tbBanIP.Name = "tbBanIP";
            this.tbBanIP.Size = new System.Drawing.Size(324, 20);
            this.tbBanIP.TabIndex = 11;
            // 
            // btnAddBan
            // 
            this.btnAddBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBan.Location = new System.Drawing.Point(345, 316);
            this.btnAddBan.Name = "btnAddBan";
            this.btnAddBan.Size = new System.Drawing.Size(103, 20);
            this.btnAddBan.TabIndex = 12;
            this.btnAddBan.Text = "Add";
            this.btnAddBan.UseVisualStyleBackColor = true;
            this.btnAddBan.Click += new System.EventHandler(this.btnAddBan_Click);
            // 
            // frmIPBans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 389);
            this.Controls.Add(this.btnAddBan);
            this.Controls.Add(this.tbBanIP);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lbIPBans);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmIPBans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IP-Ban mangement";
            this.Load += new System.EventHandler(this.frmIPBans_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbIPBans;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbBanIP;
        private System.Windows.Forms.Button btnAddBan;
    }
}