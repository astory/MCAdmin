namespace MCAdmin
{
    partial class frmKitShare
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
            this.lbKits = new System.Windows.Forms.ListBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lvItems = new System.Windows.Forms.ListView();
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbKits
            // 
            this.lbKits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbKits.FormattingEnabled = true;
            this.lbKits.IntegralHeight = false;
            this.lbKits.Location = new System.Drawing.Point(12, 12);
            this.lbKits.Name = "lbKits";
            this.lbKits.Size = new System.Drawing.Size(170, 375);
            this.lbKits.TabIndex = 0;
            this.lbKits.SelectedValueChanged += new System.EventHandler(this.lbKits_SelectedValueChanged);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(503, 358);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(109, 29);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Download kit!";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
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
            this.lvItems.Enabled = false;
            this.lvItems.FullRowSelect = true;
            this.lvItems.Location = new System.Drawing.Point(188, 12);
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(424, 340);
            this.lvItems.TabIndex = 6;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            // 
            // chID
            // 
            this.chID.Text = "ID";
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 131;
            // 
            // chAmount
            // 
            this.chAmount.Text = "Amount";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(188, 358);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(122, 29);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh list";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmKitShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 399);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lbKits);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmKitShare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kit-Share";
            this.Load += new System.EventHandler(this.frmKitShare_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbKits;
        private System.Windows.Forms.Button btnDownload;
        public System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chAmount;
        private System.Windows.Forms.Button btnRefresh;
    }
}