namespace MCAdmin
{
    partial class frmDonate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDonate));
            this.lblDonate = new System.Windows.Forms.Label();
            this.btnDonate = new System.Windows.Forms.PictureBox();
            this.btnNo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnDonate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDonate
            // 
            this.lblDonate.AutoSize = true;
            this.lblDonate.Location = new System.Drawing.Point(12, 9);
            this.lblDonate.Name = "lblDonate";
            this.lblDonate.Size = new System.Drawing.Size(375, 104);
            this.lblDonate.TabIndex = 0;
            this.lblDonate.Text = resources.GetString("lblDonate.Text");
            this.lblDonate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDonate
            // 
            this.btnDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDonate.Image = global::MCAdmin.Properties.Resources.btn_donate_LG;
            this.btnDonate.Location = new System.Drawing.Point(154, 125);
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(95, 25);
            this.btnDonate.TabIndex = 1;
            this.btnDonate.TabStop = false;
            this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
            // 
            // btnNo
            // 
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Location = new System.Drawing.Point(265, 125);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(122, 25);
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "I don\'t want to donate";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // frmDonate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 157);
            this.ControlBox = false;
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnDonate);
            this.Controls.Add(this.lblDonate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDonate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Donate";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDonate_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.btnDonate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDonate;
        private System.Windows.Forms.PictureBox btnDonate;
        private System.Windows.Forms.Button btnNo;
    }
}