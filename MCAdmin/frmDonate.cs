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
    public partial class frmDonate : Form
    {
        public frmDonate()
        {
            InitializeComponent();
        }

        private void frmDonate_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.IO.File.WriteAllText("donation-code.txt", "done");
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N75E8XW5N2XRU");
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
