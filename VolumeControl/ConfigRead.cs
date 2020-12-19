using System;
using System.IO;

namespace VolumeControl
{
    public partial class ConfigRead
    {
        public static void Settings()
        {
            if (File.Exists(MainForm.path) == true)
            {
                try
                {
                    string configFile = File.ReadAllText(MainForm.path);
                    string settingUp = getBetween(configFile, "1=", "/");
                    string settingDown = getBetween(configFile, "2=", "/");
                    string settingMute = getBetween(configFile, "3=", "/");
                    MainForm.volUpKey = settingUp;
                    MainForm.volDownKey = settingDown;
                    MainForm.volMuteKey = settingMute;
                }
                catch { }
            }
        }
        public static void Timer()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Refresh);
            timer.Interval = 1000;
            timer.Enabled = true;
        }
        private static void Refresh(object source, EventArgs e)
        {
            Settings();
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            return "";
        }
    }
}
