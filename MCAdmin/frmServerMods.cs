using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MCAdmin
{
    public partial class frmServerMods : Form
    {
        public frmServerMods()
        {
            InitializeComponent();
            InitMods();
        }

        public void InitMods()
        {
            int y = 12;

            this.SuspendLayout();

            foreach (ServerMod mod in Program.serverMods.Values)
            {
                GroupBox gbMod = new GroupBox(); ;
                Label lbModAuthor = new Label();
                Label lbModLink = new Label(); LinkLabel llModLink = new LinkLabel();
                Button btnInstall = new Button(); Button btnUninstall = new Button();

                gbMod.SuspendLayout();

                gbMod.Controls.Add(lbModAuthor);
                gbMod.Controls.Add(lbModLink);
                gbMod.Controls.Add(llModLink);
                gbMod.Controls.Add(btnInstall);
                gbMod.Controls.Add(btnUninstall);
                gbMod.Location = new System.Drawing.Point(12, y);
                gbMod.Size = new System.Drawing.Size(441, 94);
                gbMod.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                gbMod.Text = mod.name;

                lbModLink.Location = new System.Drawing.Point(6, y + 27);
                lbModLink.Size = new System.Drawing.Size(104, 13);
                lbModLink.Text = "Official forum thread:";

                llModLink.Location = new System.Drawing.Point(116, y + 27);
                llModLink.Size = new System.Drawing.Size(301, 13);
                llModLink.Text = mod.link;
                llModLink.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                llModLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(llRunecraft_LinkClicked);
                llModLink.Links.Add(new LinkLabel.Link(0, llModLink.Text.Length, llModLink.Text));

                lbModAuthor.AutoSize = true;
                lbModAuthor.Location = new System.Drawing.Point(6, y + 4);
                lbModAuthor.Size = new System.Drawing.Size(411, 13);
                lbModAuthor.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                lbModAuthor.Text = "Author: " + mod.author;

                btnInstall.Location = new System.Drawing.Point(6, y + 54);
                btnInstall.Size = new System.Drawing.Size(70, 23);
                btnInstall.Text = "Install";
                btnInstall.UseVisualStyleBackColor = true;
                btnInstall.Enabled = !mod.IsInstalled();
                btnInstall.Click += new EventHandler(delegate(object esender, EventArgs eargs)
                {
                    mod.Install();
                    btnInstall.Enabled = !mod.IsInstalled();
                    btnUninstall.Enabled = !btnInstall.Enabled;
                });

                btnUninstall.Location = new System.Drawing.Point(80, y + 54);
                btnUninstall.Size = new System.Drawing.Size(70, 23);
                btnUninstall.Text = "Uninstall";
                btnUninstall.UseVisualStyleBackColor = true;
                btnUninstall.Enabled = !btnInstall.Enabled;
                btnUninstall.Click += new EventHandler(delegate(object esender, EventArgs eargs)
                {
                    mod.Uninstall();
                    btnInstall.Enabled = !mod.IsInstalled();
                    btnUninstall.Enabled = !btnInstall.Enabled;
                });

                this.Controls.Add(gbMod);

                gbMod.PerformLayout();
                gbMod.ResumeLayout(true);

                y += 100;
            }

            y += 30;

            this.Size = new Size(481,y);

            this.ResumeLayout(true);
        }

        private void llRunecraft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process linkLabelProc = new Process();
            linkLabelProc.StartInfo.FileName = e.Link.LinkData.ToString();
            linkLabelProc.Start();
        }
    }
}
