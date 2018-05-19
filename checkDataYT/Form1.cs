using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkDataYT
{
    public partial class frmCheck : Form
    {
        public frmCheck()
        {
            InitializeComponent();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                
                ofdInsert.ShowDialog();
                string path = ofdInsert.FileName; // csv inserted.
                int count = File.ReadLines(path).Count();
                lblCount.Text = "0/" + count;
                if (path != "")
                {
                    progressBar1.Value = 50;
                    lblReaction.Text = "Loading...";
                    List<string> listA = new List<string>(); //yt links
                    List<string> listB = new List<string>(); //other data
                    using (var reader = new StreamReader(path))
                    {

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            listA.Add(values[0]);
                            listB.Add(values[1]);
                        }

                    }

                    if (listA.Count > 0)
                    {
                        string realPath = Path.GetDirectoryName(path);
                        string enabledFile = realPath + @"\enabled.csv";
                        string disabledFile = realPath + @"\disabled.csv";
                        var csven = new StringBuilder();
                        var csvdis = new StringBuilder();
                        //foreach (var link in listA)
                        for(int i = 0; i<listA.Count; i++)
                        {
                            string link = listA[i];
                            string data = listB[i];
                            if (link.IndexOf("youtube") != -1)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    string s = client.DownloadString(link);
                                    

                                    
                                  //  File.WriteAllText(filePath, csv.ToString());
                                    if (s.IndexOf("display-message") == -1) // enabled
                                    {
                                        csven.AppendLine(link+","+data);
                                    }
                                    else //disabled
                                    {
                                        csvdis.AppendLine(link + "," + data);
                                    }
                                }
                            }
                            progressBar1.Maximum = count;
                            progressBar1.Value = (i + 1);
                            lblCount.Text = (i+1)+"/" + count;
                        }
                        File.AppendAllText(enabledFile, csven.ToString());
                        File.AppendAllText(disabledFile, csvdis.ToString());
                        lblReaction.Text = "Completed!";
                        //OK.
                    }
                    else
                    {
                        //there isn't have a yt link.
                    }
                }
            }
            catch(Exception a)
            {
                progressBar1.Value = 0;
                MessageBox.Show(a.ToString());
                lblReaction.Text = "Error!";
            }

        }
    }
}
