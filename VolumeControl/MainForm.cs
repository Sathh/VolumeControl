using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VolumeControl
{
    public partial class MainForm : Form
    {
        public static string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "settings.dat");
        public static string volUpKey { get; set; }
        public static string volDownKey { get; set; }
        public static string volMuteKey { get; set; }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (Convert.ToString((Keys)vkCode) == volUpKey)
                {
                    keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
                }
                if (Convert.ToString((Keys)vkCode) == volDownKey)
                {
                    keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
                }
                if (Convert.ToString((Keys)vkCode) == volMuteKey)
                {
                    keybd_event((byte)Keys.VolumeMute, 0, 0, 0);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        public MainForm()
        {
            InitializeComponent();
            _hookID = SetHook(_proc);
            ConfigRead.Settings();
            volUpComboBox.SelectedItem = volUpKey;
            volDownComboBox.SelectedItem = volDownKey;
            volMuteComboBox.SelectedItem = volMuteKey;
            ConfigRead.Timer();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }
        private void Form1_Resize(object sender, EventArgs e)
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
            if (volUpKey == volDownKey || volUpKey == volMuteKey || volMuteKey == volDownKey)
            {
                MessageBox.Show("Don't use same keys on different settings!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
    }
}
