using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Deep_Paging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            //System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        //Start
        private void button2_Click(object sender, EventArgs e)
        {
            StartThread();
        }

        //Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation. 
            this.backgroundWorker1.CancelAsync();
        }

        private void StartThread()
        {
            // This method runs on the main thread. 

            // Initialize the object that the background worker calls.
            Downloader DL = new Downloader();
            DL.baseAddress = textBox2.Text;
            DL.SourceFolder = label1.Text;

            // Start the asynchronous operation.
            backgroundWorker1.RunWorkerAsync(DL);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // This event handler is where the actual work is done.
            // This method runs on the background thread.

            // Get the BackgroundWorker object that raised this event.
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            // Get the Words object and call the main method.
            Downloader DL = (Downloader)e.Argument; 
            DL.DownloadPages(worker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This method runs on the main thread.
            Downloader.CurrentState state = (Downloader.CurrentState)e.UserState; 
            this.textBox1.Text = state.LinesCounted.ToString();
            this.label2.Text = state.LineMessage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This event handler is called when the background thread finishes. 
            // This method runs on the main thread. 
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("File loading canceled.");
            else
                MessageBox.Show("Finished Loading files.");
        }

        //test button
        private void button4_Click(object sender, EventArgs e)
        {
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load(@"C:\Users\rloo\Desktop\_temp1\test.xml");
            System.Xml.XmlNode mynode = xml.SelectSingleNode("response/result/doc/arr[@name='subject']");
            string myValue = mynode.InnerText;

            System.Windows.Forms.MessageBox.Show(myValue);
        }
    }
}
