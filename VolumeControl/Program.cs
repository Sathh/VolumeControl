using System;
using System.IO;
using System.Windows.Forms;

namespace VolumeControl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (File.Exists(MainForm.path))
            {
                MainForm mainForm = new MainForm();
                mainForm.WindowState = FormWindowState.Minimized;
                mainForm.ShowInTaskbar = false;
                Application.Run(mainForm);
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}