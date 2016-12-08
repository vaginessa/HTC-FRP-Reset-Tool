using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HTCFRPResetTool
{
    class Tool
    {
        static string BinDir = Path.GetTempPath() + "HTCFRPReset\\";
        Process fastboot = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = BinDir,
                FileName = BinDir + "fastboot.exe",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            }
        };

        public string GetDevice()
        {
            fastboot.StartInfo.Arguments = "devices";
            fastboot.Start();
            fastboot.WaitForExit();
            string result = "";
            while (!fastboot.StandardOutput.EndOfStream)
            {
                string str = fastboot.StandardOutput.ReadLine();
                if (str.Contains("\tfastboot"))
                    return str.Replace("\tfastboot", "");
            }
            return result;
        }
        
        public string GetKSToken()
        {
            fastboot.StartInfo.Arguments = "dump kstoken 0 0 0";
            fastboot.Start();
            fastboot.WaitForExit();
            File.Delete(BinDir + "0");
            string result = "";
            while (!fastboot.StandardError.EndOfStream)
            {
                string str = fastboot.StandardError.ReadLine();
                if (str.Contains("INFO"))
                    result += str.Replace("(bootloader) INFO", "") + "\r\n";
            }
            return result;
        }
        
        public void HTCdevWebsite()
        {
            Process.Start("http://www.htcdev.com/bootloader/");
        }

        public bool FlashKSToken(string file)
        {
            fastboot.StartInfo.Arguments = "flash kstoken " + file;
            fastboot.Start();
            fastboot.WaitForExit();
            while (!fastboot.StandardError.EndOfStream)
            {
                string str = fastboot.StandardError.ReadLine();
                if (str.Contains("Erase frp success"))
                    return true;
            }
            return false;
        }

        public void Reboot_Bootloader()
        {
            fastboot.StartInfo.Arguments = "reboot-bootloader";
            fastboot.Start();
        }

        public void Reboot_Download()
        {
            fastboot.StartInfo.Arguments = "oem reboot-download";
            fastboot.Start();
        }

        public void Reboot()
        {
            fastboot.StartInfo.Arguments = "reboot";
            fastboot.Start();
        }

        public void MyFacebook()
        {
            Process.Start("https://www.facebook.com/profile.php?id=100005653172695");
        }
    }
}
