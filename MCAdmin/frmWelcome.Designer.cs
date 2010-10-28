namespace MCAdmin
{
    partial class frmWelcome
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.cbList = new System.Windows.Forms.CheckBox();
            this.cbBans = new System.Windows.Forms.CheckBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.tbOwner = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbWhitelist = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(413, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "It seems this is the first time you run MCAdmin, so please answer these basic que" +
                "stions";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(15, 230);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(398, 29);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // cbList
            // 
            this.cbList.AutoSize = true;
            this.cbList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbList.Checked = true;
            this.cbList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbList.Location = new System.Drawing.Point(12, 35);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(274, 30);
            this.cbList.TabIndex = 5;
            this.cbList.Text = "Do you want your server to appear on the global list?\r\n(viewable on http://list.m" +
                "cadmin.eu/)";
            this.cbList.UseVisualStyleBackColor = true;
            // 
            // cbBans
            // 
            this.cbBans.AutoSize = true;
            this.cbBans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbBans.Checked = true;
            this.cbBans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBans.Location = new System.Drawing.Point(15, 71);
            this.cbBans.Name = "cbBans";
            this.cbBans.Size = new System.Drawing.Size(295, 30);
            this.cbBans.TabIndex = 7;
            this.cbBans.Text = "Do you wish the global banlist to be used on your server?\r\n(viewable on http://ba" +
                "ns.mcadmin.eu/)";
            this.cbBans.UseVisualStyleBackColor = true;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(12, 184);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(242, 26);
            this.lblOwner.TabIndex = 11;
            this.lblOwner.Text = "What is your (the owner\'s) full ingame name?\r\nLeave empty if you wish to set your" +
                "self admin later";
            // 
            // tbOwner
            // 
            this.tbOwner.Location = new System.Drawing.Point(260, 190);
            this.tbOwner.Name = "tbOwner";
            this.tbOwner.Size = new System.Drawing.Size(153, 20);
            this.tbOwner.TabIndex = 12;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 154);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(163, 13);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "What is the name of your server?";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(181, 151);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(232, 20);
            this.tbName.TabIndex = 10;
            this.tbName.Text = "MCAdmin Server";
            // 
            // cbWhitelist
            // 
            this.cbWhitelist.AutoSize = true;
            this.cbWhitelist.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbWhitelist.Location = new System.Drawing.Point(15, 107);
            this.cbWhitelist.Name = "cbWhitelist";
            this.cbWhitelist.Size = new System.Drawing.Size(257, 30);
            this.cbWhitelist.TabIndex = 8;
            this.cbWhitelist.Text = "Do you want to run your server in whitelist mode?\r\n(Recommended for private serve" +
                "rs)";
            this.cbWhitelist.UseVisualStyleBackColor = true;
            // 
            // frmWelcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 271);
            this.ControlBox = false;
            this.Controls.Add(this.tbOwner);
            this.Controls.Add(this.lblOwner);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cbWhitelist);
            this.Controls.Add(this.cbBans);
            this.Controls.Add(this.cbList);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmWelcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to MCAdmin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.CheckBox cbList;
        private System.Windows.Forms.CheckBox cbBans;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.TextBox tbOwner;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cbWhitelist;
    }
}