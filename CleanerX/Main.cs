using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Windows.Forms;

namespace CleanerX
{
    public partial class Main : Form
    {
        [DllImport("psapi")]
        public static extern int EmptyWorkingSet(IntPtr handle);
        public Main()
        {
            InitializeComponent();
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("CleanerX", Application.ExecutablePath);
            ServiceController controller;
            try
            {
                controller = new ServiceController("XblAuthManager");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("XblGameSave");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("XboxGip");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("XboxGipSvc");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("XboxNetApiSvc");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("edgeupdate");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("edgeupdatem");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("wuauserv");
                controller.Stop();
            }
            catch { }
            try
            {
                controller = new ServiceController("WaaSMedicSvc");
                controller.Stop();
            }
            catch { }
            DirectoryInfo directory = new DirectoryInfo(Path.GetTempPath());
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }
            directory = new DirectoryInfo(Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine));
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
            decimal percentOccupied = 100 - percentFree;
            if (Math.Round(percentOccupied) > 59)
            {
                Process[] process = Process.GetProcesses();
                foreach (Process p in process) try { EmptyWorkingSet(p.Handle); } catch { }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, this.exit_Click);
        }

        void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
    public static class PerformanceInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static Int64 GetPhysicalAvailableMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }
        }

        public static Int64 GetTotalMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }
        }
    }
}
