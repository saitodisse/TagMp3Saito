using System;
using System.Windows.Forms;

namespace TagMp3Saito_WindowsFormsApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmTagMp3Saito());
        }
    }
}