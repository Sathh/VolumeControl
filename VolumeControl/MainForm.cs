using System;
using System.IO;
using System.Windows.Forms;

namespace VolumeControl
{
    public partial class MainForm : Form
    {
        public static string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "settings.dat");
        public static string volUpKey { get; set; }
        public static string volDownKey { get; set; }
        public static string volMuteKey { get; set; }
        public MainForm()
        {
            InitializeComponent();
            Hook._hookID = Hook.SetHook(Hook._proc);
            ConfigRead.Settings();
            volUpComboBox.SelectedItem = volUpKey;
            volDownComboBox.SelectedItem = volDownKey;
            volMuteComboBox.SelectedItem = volMuteKey;
            ConfigRead.Timer();
        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }
        private void KeyBind(object sender, EventArgs e)
        {
            string volUpKey = volUpComboBox.SelectedItem.ToString();
            string volDownKey = volDownComboBox.SelectedItem.ToString();
            string volMuteKey = volMuteComboBox.SelectedItem.ToString();
            string source = volUpKey + volDownKey + volMuteKey;
            int count = 0;
            foreach (char s in source)
            {
                if (s == ('N'))
                {
                    count++;
                }
            }
            if (count < 2)
            {
                if (volUpKey == volDownKey || volUpKey == volMuteKey || volMuteKey == volDownKey)
                {
                    MessageBox.Show("Don't use same keys on different settings!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string config = ("1=" + volUpKey + "/" + "2=" + volDownKey + "/" + "3=" + volMuteKey + "/");
            File.WriteAllText(path, config);
        }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hook.UnhookWindowsHookEx(Hook._hookID);
            restoreToolStripMenuItem.Visible = true;
            pauseToolStripMenuItem.Visible = false;
        }
        private void restoreToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Hook._hookID = Hook.SetHook(Hook._proc);
            pauseToolStripMenuItem.Visible = true;
            restoreToolStripMenuItem.Visible = false;
        }
    }
}
