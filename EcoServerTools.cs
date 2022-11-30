using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace AlteredEco
{
    public partial class EcoServerTools : Form
    {
        public object selected;
        public int selectedhour;
        public int selectedmin;
        public static bool closing = false;
        public DateTime lastRestart { get; set; }
        public bool check1 { get; set; } = Properties.Settings.Default.autostart;
        public bool check2 { get; set; } = Properties.Settings.Default.autoreboot;
        public int hour { get; set; } = Properties.Settings.Default.hours;
        public int min { get; set; } = Properties.Settings.Default.mins;
        public bool run { get; set; } = Properties.Settings.Default.startupRun;
        public static string dirloc { get; set; } = Properties.Settings.Default.directory;
        public bool tooltips_on { get; set; } = Properties.Settings.Default.tooltips;
        public EcoServerTools()
        {
            InitializeComponent();
            Timer MyTimer = new Timer();
            MyTimer.Interval = (1000);
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
            checkBox1.Checked = check1;
            rebootcheck.Checked = check2;
            hours.SelectedIndex = hour;
            minutes.SelectedIndex = min;
            chkStartUp.Checked = run;
            textBox1.Text = dirloc;
            checkBox2.Checked = tooltips_on;
        }

        private void EcoServerTools_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.Location;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (chkStartUp.Checked && rk.GetValue(Process.GetCurrentProcess().MainModule.FileName) == null)
                rk.SetValue(Process.GetCurrentProcess().MainModule.FileName, Application.ExecutablePath);
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            label1.Text = time.ToString();
            label7.Text = "Last Restart: " + Properties.Settings.Default.lastrestart;
            if (time.Hour <= 6 || time.Hour >= 18)
            {
                this.BackColor = Color.FromArgb(0,0,60);
            }
            else
            {
                this.BackColor = Color.FromArgb(0, 0, 60);
            }
            if (CheckRunning() && !closing)
            {
                label2.Text = "Eco is Running";
                label2.ForeColor = Color.Lime;
            }
            else if (!CheckRunning() && !closing)
            {
                label2.Text = "Eco is not Running";
                label2.ForeColor = Color.Red;
                if (checkBox1.Checked && !closing)
                    StartEco();
            }
            else if (CheckResponding() && !closing)
            {
                label2.Text = "Eco is not Responding";
                label2.ForeColor = Color.Red;
                if (checkBox1.Checked && !closing)
                    StartEco();
            }
            if (selectedhour == time.Hour && selectedmin == time.Minute && time.Second == 0 && rebootcheck.Checked)
            {
                lastRestart = DateTime.Now;
                Properties.Settings.Default.lastrestart = lastRestart.ToString();
                Properties.Settings.Default.Save();
                label2.Text = "Restarting Server";
                label2.ForeColor = Color.White;
                closing = true;
                CloseEco();
                Restart();
            }
            Properties.Settings.Default.Location = Location;
            Properties.Settings.Default.Save();

        }

        public static void LogOff()
        {
            StartShutDown("-l");
        }

        public static void Shut()
        {
            StartShutDown("-f -s -t 20");
        }

        public static void Restart()
        {
            StartShutDown("-f -r -t 20");
        }

        private static void StartShutDown(string param)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/C shutdown " + param;
            Process.Start(proc);
        }

        public static bool CheckRunning()
        {
            if (Process.GetProcessesByName("EcoServer").Length > 0)
            { 
                return true;
            }
            return false;
        }

        public static bool CheckResponding()
        {
            Process[] processes = null;
            processes = Process.GetProcessesByName("EcoServer");
            foreach (Process process in processes)
            {
                if (process.Responding)
                {
                    return true;
                }
            }
            return false;
        }

        private void StartEco()
        {
            ProcessStartInfo eco = new ProcessStartInfo();
            eco.WorkingDirectory = dirloc;
            eco.FileName = "EcoServer.exe";
            if (Directory.Exists(dirloc) && File.Exists(dirloc + "EcoServer.exe"))
                Process.Start(eco);
        }

        private static void CloseEco()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/c taskkill/im EcoServer.exe";
            Process.Start(proc);
        }

        private void minutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedmin = minutes.SelectedIndex;
            Properties.Settings.Default.mins = selectedmin;
            Properties.Settings.Default.Save();
        }

        private void hours_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedhour = hours.SelectedIndex;
            Properties.Settings.Default.hours = selectedhour;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autostart = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void rebootcheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autoreboot = rebootcheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkStartUp_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.startupRun = chkStartUp.Checked;
            Properties.Settings.Default.Save();

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (chkStartUp.Checked)
                rk.SetValue(Process.GetCurrentProcess().MainModule.FileName, Application.ExecutablePath);
            else
                rk.DeleteValue(Process.GetCurrentProcess().MainModule.FileName, false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dirloc = textBox1.Text;
            Properties.Settings.Default.directory = dirloc;
            Properties.Settings.Default.Save();
            if (File.Exists(dirloc + "EcoServer.exe"))
            {
                label9.Text = "";
                linkLabel1.Text = "Open Server Folder";
            }
            else
            {
                label9.Text = "path does not exist";
                linkLabel1.Text = "";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(dirloc))
                Process.Start("explorer.exe", dirloc);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            tooltips_on = checkBox2.Checked;
            Properties.Settings.Default.tooltips = tooltips_on;
            Properties.Settings.Default.Save();
            if (checkBox2.Checked)
                toolTip1.Active = true;
            else
                toolTip1.Active = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.SelectedPath = dirloc;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string modifiedloc = fbd.SelectedPath + @"\";
                    textBox1.Text = modifiedloc;
                }
            }
        }
    }
}
