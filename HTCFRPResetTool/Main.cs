using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HTCFRPResetTool
{
    public partial class Main : Form
    {
        String BinDir = Path.GetTempPath() + "HTCFRPReset\\";
        Tool tool = new Tool();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(BinDir)) Directory.CreateDirectory(BinDir);

            var Assembly = System.Reflection.Assembly.GetExecutingAssembly();

            using (FileStream fs1 = File.Create(BinDir + "AdbWinApi.dll"))
                Assembly.GetManifestResourceStream("HTCFRPResetTool.Binary.AdbWinApi.dll").CopyTo(fs1);
            using (FileStream fs2 = File.Create(BinDir + "fastboot.exe"))
                Assembly.GetManifestResourceStream("HTCFRPResetTool.Binary.fastboot.exe").CopyTo(fs2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string device = tool.GetDevice();
            groupBox1.Text = "Device:" + device;
            groupBox1.Enabled = button1.Enabled = button3.Enabled = !string.IsNullOrEmpty(device);
            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = tool.GetKSToken();
            if (!string.IsNullOrEmpty(str))
            {
                Clipboard.SetText(str);
                textBox1.Text = str;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tool.HTCdevWebsite();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "正在清除 FRP, 請稍後...\r\n";
                MessageBox.Show("清除 FRP " + (tool.FlashKSToken(openFileDialog1.FileName) ? "成功" : "失敗"));
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tool.MyFacebook();
        }

        private void btn_bootloader_Click(object sender, EventArgs e)
        {
            tool.Reboot_Bootloader();
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            tool.Reboot_Download();
        }

        private void btn_reboot_Click(object sender, EventArgs e)
        {
            tool.Reboot();
        }
    }
}
