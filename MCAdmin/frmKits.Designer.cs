namespace MCAdmin
{
    partial class frmKits
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
            this.cbKit = new System.Windows.Forms.ComboBox();
            this.btnNewKit = new System.Windows.Forms.Button();
            this.lblKit = new System.Windows.Forms.Label();
            this.lblReqLevel = new System.Windows.Forms.Label();
            this.cbReqLevel = new System.Windows.Forms.ComboBox();
            this.lvItems = new System.Windows.Forms.ListView();
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblItems = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnDelItem = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbKit
            // 
            this.cbKit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKit.FormattingEnabled = true;
            this.cbKit.Location = new System.Drawing.Point(31, 12);
            this.cbKit.Name = "cbKit";
            this.cbKit.Size = new System.Drawing.Size(373, 21);
            this.cbKit.TabIndex = 0;
            this.cbKit.SelectedIndexChanged += new System.EventHandler(this.cbKit_SelectedIndexChanged);
            // 
            // btnNewKit
            // 
            this.btnNewKit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewKit.Location = new System.Drawing.Point(410, 12);
            this.btnNewKit.Name = "btnNewKit";
            this.btnNewKit.Size = new System.Drawing.Size(98, 21);
            this.btnNewKit.TabIndex = 1;
            this.btnNewKit.Text = "New";
            this.btnNewKit.UseVisualStyleBackColor = true;
            this.btnNewKit.Click += new System.EventHandler(this.btnNewKit_Click);
            // 
            // lblKit
            // 
            this.lblKit.AutoSize = true;
            this.lblKit.Location = new System.Drawing.Point(3, 15);
            this.lblKit.Name = "lblKit";
            this.lblKit.Size = new System.Drawing.Size(22, 13);
            this.lblKit.TabIndex = 2;
            this.lblKit.Text = "Kit:";
            // 
            // lblReqLevel
            // 
            this.lblReqLevel.AutoSize = true;
            this.lblReqLevel.Location = new System.Drawing.Point(12, 48);
            this.lblReqLevel.Name = "lblReqLevel";
            this.lblReqLevel.Size = new System.Drawing.Size(78, 13);
            this.lblReqLevel.TabIndex = 3;
            this.lblReqLevel.Text = "Required level:";
            // 
            // cbReqLevel
            // 
            this.cbReqLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbReqLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReqLevel.FormattingEnabled = true;
            this.cbReqLevel.Location = new System.Drawing.Point(96, 45);
            this.cbReqLevel.Name = "cbReqLevel";
            this.cbReqLevel.Size = new System.Drawing.Size(516, 21);
            this.cbReqLevel.TabIndex = 4;
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chName,
            this.chAmount});
            this.lvItems.FullRowSelect = true;
            this.lvItems.Location = new System.Drawing.Point(12, 99);
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(600, 305);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
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
            // chAmount
            // 
            this.chAmount.Text = "Amount";
            // 
            // lblItems
            // 
            this.lblItems.AutoSize = true;
            this.lblItems.Location = new System.Drawing.Point(12, 78);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new System.Drawing.Size(35, 13);
            this.lblItems.TabIndex = 6;
            this.lblItems.Text = "Items:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 410);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Size = new System.Drawing.Size(600, 35);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(291, 27);
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
            this.btnSave.Size = new System.Drawing.Size(291, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(514, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(98, 21);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(469, 72);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(143, 21);
            this.btnImport.TabIndex = 15;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(53, 72);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 24);
            this.btnAddItem.TabIndex = 16;
            this.btnAddItem.Text = "+";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnDelItem
            // 
            this.btnDelItem.Location = new System.Drawing.Point(82, 72);
            this.btnDelItem.Name = "btnDelItem";
            this.btnDelItem.Size = new System.Drawing.Size(23, 24);
            this.btnDelItem.TabIndex = 17;
            this.btnDelItem.Text = "-";
            this.btnDelItem.UseVisualStyleBackColor = true;
            this.btnDelItem.Click += new System.EventHandler(this.btnDelItem_Click);
            // 
            // frmKits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 457);
            this.Controls.Add(this.btnDelItem);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lblItems);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.cbReqLevel);
            this.Controls.Add(this.lblReqLevel);
            this.Controls.Add(this.lblKit);
            this.Controls.Add(this.btnNewKit);
            this.Controls.Add(this.cbKit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmKits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kit mangement";
            this.Load += new System.EventHandler(this.frmKits_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewKit;
        private System.Windows.Forms.Label lblKit;
        public System.Windows.Forms.ComboBox cbKit;
        private System.Windows.Forms.Label lblReqLevel;
        private System.Windows.Forms.ComboBox cbReqLevel;
        private System.Windows.Forms.Label lblItems;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnDelItem;
        public System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.ColumnHeader chAmount;
    }
}