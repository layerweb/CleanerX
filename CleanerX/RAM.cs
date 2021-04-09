using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CleanerX
{
    public partial class RAM : Form
    {
        public RAM()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RAM_Load(object sender, EventArgs e)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\CleanerX", true);
            comboBox1.SelectedIndex = comboBox1.FindStringExact(key.GetValue("RAM") + "%");
            key.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\CleanerX", true);
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    key.SetValue("RAM", "20");
                    break;
                case 1:
                    key.SetValue("RAM", "25");
                    break;
                case 2:
                    key.SetValue("RAM", "30");
                    break;
                case 3:
                    key.SetValue("RAM", "35");
                    break;
                case 4:
                    key.SetValue("RAM", "40");
                    break;
                case 5:
                    key.SetValue("RAM", "45");
                    break;
                case 6:
                    key.SetValue("RAM", "50");
                    break;
                case 7:
                    key.SetValue("RAM", "55");
                    break;
                case 8:
                    key.SetValue("RAM", "60");
                    break;
                case 9:
                    key.SetValue("RAM", "65");
                    break;
                case 10:
                    key.SetValue("RAM", "70");
                    break;
                case 11:
                    key.SetValue("RAM", "75");
                    break;
                case 12:
                    key.SetValue("RAM", "80");
                    break;
                case 13:
                    key.SetValue("RAM", "85");
                    break;
                case 14:
                    key.SetValue("RAM", "90");
                    break;
            }
            Globals.RAMPercent = Convert.ToInt32(key.GetValue("RAM")) - 1;
            key.Close();
        }
    }
}
