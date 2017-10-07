using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioIntelligence
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                string path = d.Name;
                //TreeScan("C:\\");
                checkedListBox1.Items.Add(path, CheckState.Checked);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            nbfiles = 0;
            nbfiles_mp3 = 0;
            mp3_total_len = 0;

            //TreeScan("F:\\AUDIO");

            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string path = checkedListBox1.CheckedItems[i].ToString();
                log("Scanning path = " + path);
                TreeScan(path);
            }
            long milliseconds2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            log("Done : " + nbfiles + " files in " + (milliseconds2-milliseconds) + " ms.");
            log("Done : " + nbfiles_mp3 + " mp3 files.");
            log("total mp3 length b = " + mp3_total_len);
            log("total mp3 length kb = " + mp3_total_len/1024);
            log("total mp3 length mb = " + mp3_total_len / 1024 / 1024);
            log("total mp3 length gb = " + mp3_total_len / 1024 / 1024 / 1024);


        }

        private void log(string str)
        {
            listBox1.Items.Add(str);
        }

        int nbfiles = 0;
        int nbfiles_mp3 = 0;
        long mp3_total_len = 0;

        private void TreeScan(string sDir)
        {
            bool authorized = true;
            try
            {
                Directory.GetFiles(sDir);
            }
            catch (UnauthorizedAccessException)
            {
                authorized = false;
                //listBox1.Items.Add("Unauthorized : " + sDir);
            }

            if (authorized == false)
            {
                return;
            }

            string[] files = Directory.GetFiles(sDir);
            if (files.Count() == 0)
            {
                //log("no files in folrder : " + sDir);
                return;
            }

            foreach (string f in Directory.GetFiles(sDir))
            {

                nbfiles++;
                if (f.ToLower().EndsWith(".mp3"))
                {
                    long length = new System.IO.FileInfo(f).Length;
                    mp3_total_len += length;
                    nbfiles_mp3++;
                    //log(f + " " + length);
                }
            }

            foreach (string d in Directory.GetDirectories(sDir))
                TreeScan(d); // recursive call to get files of directory
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
