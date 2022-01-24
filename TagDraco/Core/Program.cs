using System;
using System.Windows.Forms;
using TagDraco.GUI;

namespace TagDraco.Core
{
    static class Program
    {
        public static string VERSION;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                VERSION = "debug";
            }
            else
            {
                VERSION = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGUI());
        }
    }
}
