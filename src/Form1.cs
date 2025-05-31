using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace VTuploader
{
    public partial class VTUploader : Form
    {
        string lang = CultureInfo.CurrentCulture.EnglishName;
        public VTUploader(string[] args)
        {
            InitializeComponent();
            label1.Text = lang.Contains("Russia") ? "Необходим api ключ\nВставь его в строку ниже." : "API key required\nPaste it in the line below.";
            button1.Text = lang.Contains("Russia") ? "Продолжить" : "Continue";
            programpath = args[0];
        }
        string programpath;
        private void button1_Click(object sender, EventArgs e)
        { 
            //проверяем что в строке действительно апи ключик
            if (textBox1.TextLength == 64)
            {
                Registry.CurrentUser.OpenSubKey("Environment", true).SetValue("VT_key", textBox1.Text);
                Process.Start(new ProcessStartInfo { FileName = Assembly.GetExecutingAssembly().Location, Arguments = $"\"{programpath}\"" });
                Environment.Exit(0);
            }
        }
    }
}
