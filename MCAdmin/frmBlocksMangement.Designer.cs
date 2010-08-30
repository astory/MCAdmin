namespace MCAdmin
{
    partial class frmBlocksMangement
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvBlocks = new System.Windows.Forms.ListView();
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblMode = new System.Windows.Forms.Label();
            this.rbWhitelist = new System.Windows.Forms.RadioButton();
            this.rbBlacklist = new System.Windows.Forms.RadioButton();
            this.chLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDelItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.lblItems = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 387);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Size = new System.Drawing.Size(568, 35);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(275, 27);
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
            this.btnSave.Size = new System.Drawing.Size(275, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lvBlocks
            // 
            this.lvBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBlocks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chName,
            this.chLevel});
            this.lvBlocks.FullRowSelect = true;
            this.lvBlocks.Location = new System.Drawing.Point(12, 60);
            this.lvBlocks.MultiSelect = false;
            this.lvBlocks.Name = "lvBlocks";
            this.lvBlocks.Size = new System.Drawing.Size(571, 321);
            this.lvBlocks.TabIndex = 13;
            this.lvBlocks.UseCompatibleStateImageBehavior = false;
            this.lvBlocks.View = System.Windows.Forms.View.Details;
            this.lvBlocks.DoubleClick += new System.EventHandler(this.lvBlocks_DoubleClick);
            this.lvBlocks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvBlocks_KeyDown);
            // 
            // chID
            // 
            this.chID.Text = "ID";
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 388;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(12, 9);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(37, 13);
            this.lblMode.TabIndex = 17;
            this.lblMode.Text = "Mode:";
            // 
            // rbWhitelist
            // 
            this.rbWhitelist.AutoSize = true;
            this.rbWhitelist.Location = new System.Drawing.Point(55, 7);
            this.rbWhitelist.Name = "rbWhitelist";
            this.rbWhitelist.Size = new System.Drawing.Size(172, 17);
            this.rbWhitelist.TabIndex = 18;
            this.rbWhitelist.Text = "Whitelist (default level is admin)";
            this.rbWhitelist.UseVisualStyleBackColor = true;
            // 
            // rbBlacklist
            // 
            this.rbBlacklist.AutoSize = true;
            this.rbBlacklist.Checked = true;
            this.rbBlacklist.Location = new System.Drawing.Point(233, 7);
            this.rbBlacklist.Name = "rbBlacklist";
            this.rbBlacklist.Size = new System.Drawing.Size(169, 17);
            this.rbBlacklist.TabIndex = 19;
            this.rbBlacklist.TabStop = true;
            this.rbBlacklist.Text = "Blacklist (default level is guest)";
            this.rbBlacklist.UseVisualStyleBackColor = true;
            // 
            // chLevel
            // 
            this.chLevel.Text = "Required level";
            // 
            // btnDelItem
            // 
            this.btnDelItem.Location = new System.Drawing.Point(82, 30);
            this.btnDelItem.Name = "btnDelItem";
            this.btnDelItem.Size = new System.Drawing.Size(23, 24);
            this.btnDelItem.TabIndex = 22;
            this.btnDelItem.Text = "-";
            this.btnDelItem.UseVisualStyleBackColor = true;
            this.btnDelItem.Click += new System.EventHandler(this.btnDelItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(53, 30);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 24);
            this.btnAddItem.TabIndex = 21;
            this.btnAddItem.Text = "+";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // lblItems
            // 
            this.lblItems.AutoSize = true;
            this.lblItems.Location = new System.Drawing.Point(12, 36);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new System.Drawing.Size(35, 13);
            this.lblItems.TabIndex = 20;
            this.lblItems.Text = "Items:";
            // 
            // frmBlocksMangement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 434);
            this.Controls.Add(this.btnDelItem);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.lblItems);
            this.Controls.Add(this.rbBlacklist);
            this.Controls.Add(this.rbWhitelist);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lvBlocks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmBlocksMangement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Block management";
            this.Load += new System.EventHandler(this.frmBlocksMangement_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.ListView lvBlocks;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.RadioButton rbWhitelist;
        private System.Windows.Forms.RadioButton rbBlacklist;
        private System.Windows.Forms.ColumnHeader chLevel;
        private System.Windows.Forms.Button btnDelItem;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label lblItems;

    }
}