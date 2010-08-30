using System;
using System.Windows.Forms;

namespace MCAdmin
{
    static class Program
    {
        public static bool dontUpdate = false;
        public static bool dontUpdateMCAdmin = false;
        public static bool dontUpdateJAR = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.ToLower().Contains("noupdate")) dontUpdate = true;
                else if (arg.ToLower().Contains("nojarupdate")) dontUpdateJAR = true;
                else if (arg.ToLower().Contains("noexeupdate")) dontUpdateMCAdmin = true;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
