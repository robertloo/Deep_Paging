using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deep_Paging
{
    class Downloader
    {
        // Object to store the current state, for passing to the caller. 
        public class CurrentState
        {
            public int LinesCounted; // this goes to textBox1.Text  You see it as 'state.LinesCounted'
            public string LineMessage;
        }

        public string baseAddress;
        public string SourceFolder; // this takes form label1.Text
        private int LinesCounted;  // you see this as LinesCounted

        public void DownloadPages(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentState state = new CurrentState();
            System.Net.WebClient client = new System.Net.WebClient();

            string cursorMark = string.Empty;
            string nextCursorMark = "*";
            string path = System.IO.Path.GetFullPath(SourceFolder);
            string dest = string.Empty;

            if (System.IO.Directory.Exists(SourceFolder) == false)
            {
                throw new Exception("SourceFolder not specified.");
            }

            //Creates results folder if it doesn't exist already
            string results = path + "\\results";
            if (!System.IO.Directory.Exists(results))
            {
                System.IO.Directory.CreateDirectory(path + "\\results");
            }
          
            while (cursorMark != nextCursorMark)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    LinesCounted += 1;
                    dest = path + @"\results\File_" + LinesCounted + ".xml";

                    while (!System.IO.File.Exists(dest))
                    {
                        if (worker.CancellationPending)
                        {
                            break;
                        }
                        else
                        {                  
                            try
                            {
                                cursorMark = nextCursorMark;
                                client.DownloadFile(baseAddress + "&cursorMark=" + cursorMark, dest);
                            }
                            catch (System.Net.WebException exeption)
                            {
                                state.LineMessage = exeption.Message
                                    + "\n" + exeption.InnerException.Message
                                    + "\nFollowing file was not found"
                                    + "\n" + baseAddress + "&curserMark=" + cursorMark
                                    + "\nWill try again in 2 seconds";

                                worker.ReportProgress(0, state);
                                System.Threading.Thread.Sleep(2000); // try again in x milliseconds 
                            }
                        }
                    }                    
                    state.LinesCounted = LinesCounted;
                    state.LineMessage = "finished processing:\n\n" + dest;

                    worker.ReportProgress(0, state);

                    System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load(dest);
                    nextCursorMark = xdoc.Root.Element("str").Value;
                    //System.Threading.Thread.Sleep(2000);
                }  
            }
            state.LineMessage = "complete!";
            worker.ReportProgress(0, state);
        }
    }
}
