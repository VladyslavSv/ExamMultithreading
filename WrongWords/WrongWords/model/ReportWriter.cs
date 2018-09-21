using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WrongWords.model
{
    class ReportWriter
    {
        private string pathToReport;

        public ReportWriter(string pathToReport)
        {
            this.pathToReport = pathToReport;
        }

        public void startWritingReport()
        {
            using (StreamWriter writer = new StreamWriter(pathToReport, false))
            {
                writer.Write("----------");
                writer.Write(DateTime.Now);
                writer.WriteLine("----------");
            }
        }
        public void addFileToReport(Dictionary<string, int> localDictionary, FileInfo fileInfo)
        {
            using (StreamWriter writer = new StreamWriter(pathToReport, true))
            {
                writer.WriteLine(fileInfo.FullName);
                writer.WriteLine("File length: " + fileInfo.Length.ToString());
                writer.WriteLine("Words: ");

                    foreach (KeyValuePair<string, int> pair in localDictionary)
                    {
                        if (pair.Value != 0)
                        {
                            writer.WriteLine(pair.Key + " - " + pair.Value);
                        }
                    }
                
            }
        }

        public void endWritingReport(Dictionary<string, int> allWords)
        {
            List<KeyValuePair<string, int>> myList = null;

            myList = allWords.ToList();
            

            myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            using (StreamWriter writer = new StreamWriter(pathToReport, true))
            {

                writer.WriteLine("-------------------top 10--------------------");

                int maxSize = (myList.Count >= 10) ? 10 : myList.Count;

                for (int i = 0; i < maxSize; i++)
                {
                    writer.WriteLine(myList[i].Key + ":" + myList[i].Value);
                }

                writer.WriteLine("---------------------------------------------");
            }

        }
    }
}
