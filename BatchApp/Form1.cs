using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        string ROOT_COMMAND = string.Empty;
        const string COMMAND = @"xcopy /e";
        const string TARGETPATH = @"C:\WElVi\backup";
        private void Form1_Load(object sender, EventArgs e)
        {
            var today = DateTime.Now.ToString("yyyyMMddhhmm");
            Visible = false;
            ROOT_COMMAND = Path.Combine(@"C:\WElVi\backup", today);
            if (!Directory.Exists(ROOT_COMMAND))
            {
                Directory.CreateDirectory(ROOT_COMMAND);
            }

            string sourceFilename = string.Empty;
            using (FolderBrowserDialog openDlg = new FolderBrowserDialog())
            {
                openDlg.ShowDialog();
                sourceFilename = openDlg.SelectedPath;


                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.OutputDataReceived += p_OutputDataReceived;
                p.ErrorDataReceived += p_ErrorDataReceived;
                p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
                p.StartInfo.Arguments = @"/c " + COMMAND + " " + sourceFilename + " " + Path.Combine(TARGETPATH, today);
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
                p.Close();
                Application.Exit();
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}