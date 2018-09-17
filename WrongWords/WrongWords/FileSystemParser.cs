using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Threading;

namespace WrongWords
{
    public class FileSystemParser
    {
        private Dictionary<string, int> allWords = new Dictionary<string, int>();
        private string swapPattern = "*******";
        private string pathToReport = "report.txt";
        private int counter = 1;
        private MainWindow window
        {
            get;set;
        }

        public string directoryForCopy
        {
            get;set;
        }

        public FileSystemParser() { }

        public FileSystemParser(MainWindow window)
        {
            this.window = window;
        }

        public FileSystemParser(string pathToFileWithKeyWords, string directoryForCopy)
        {
            initKeyWords(pathToFileWithKeyWords);

            this.directoryForCopy = directoryForCopy;
        }


        public async void parseFiles()
        {
           
            startWritingReport();

             List<Task> tasks = new List<Task>();

            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                tasks.Add(parseDirectory(driveInfo.Name));
            }

            await Task.Run(() => {
                Task.WaitAll(tasks.ToArray());
                MessageBox.Show("all done");
                endWritingReport();
            });      
        }
       

        public async Task parseDirectory(string path)
        {
           await Task.Run( async () =>
           {
               Dictionary<string, int> localDictionary = null;
                string[] allLines = new string[0];
                Regex regex = null;
                StreamWriter writer = null;
                string newFileName = "";
                int replaceIndex = 0;


                try
                {
                    //пройдемся по папкам в текущей директории
                    foreach (FileInfo innerInfo in new DirectoryInfo(path).GetFiles("*.txt"))
                    {

                        localDictionary = new Dictionary<string, int>();
                        this.copyKeys(localDictionary);
                        //получим все строки файла
                        allLines = File.ReadAllLines(innerInfo.FullName);

                        //пройдемся по всем строкам файла
                        for (int i = 0; i < allLines.Length; i++)
                        {
                            lock (allWords)
                            {
                                //заменим все вхождения каждого слова в строке
                                foreach (KeyValuePair<string, int> keyValue in allWords)
                                {
                                    //заменяем пока встречается слово
                                    while (allLines[i].IndexOf(keyValue.Key) != -1)
                                    {
                                        regex = new Regex(keyValue.Key);

                                        string before = allLines[i];

                                        allLines[i] = regex.Replace(allLines[i], swapPattern, 1);

                                        if (allLines[i] != before)
                                        {
                                            replaceIndex++;
                                            if (replaceIndex == 1)
                                            {
                                                newFileName = makeCorrectedFileName(innerInfo.FullName);
                                                writer = new StreamWriter(newFileName);
                                            }
                                        }
                                        localDictionary[keyValue.Key]++;

                                       window.uiCounter++;
                                    }
                                }
                            }

                            if (replaceIndex != 0)
                            {
                                writer.Write(allLines[i]);
                            }


                        }
                        if (replaceIndex >= 1)
                        {
                            File.Copy(innerInfo.FullName, replacePath(innerInfo.FullName), true);

                            replaceIndex = 0;

                            writer.Flush();
                            writer.Close();

                            lock (allWords)
                            {
                                //добавим встречающиеся слова в общий зачет
                                foreach (KeyValuePair<string, int> pair in localDictionary)
                                {
                                    allWords[pair.Key] += localDictionary[pair.Key];
                                }
                            }
                            //добавим запись в отчет
                            addFileToReport(localDictionary, innerInfo);
                            addFileToListView(localDictionary, innerInfo);
                        }


                    }

                    foreach (DirectoryInfo directoryInfo in new DirectoryInfo(path).GetDirectories())
                    {
                           await parseDirectory(directoryInfo.FullName);       
                    }
                }
                catch (UnauthorizedAccessException unauthorizedException)
                {
                    return;
                }
                catch (IOException ex)
                {
                    return;
                }
           });
        }
        public bool isPathRoot(string path)
        {
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (path == driveInfo.Name)
                {
                    return true;
                }
            }

            return false;
        }
        public void initKeyWords(string path)
        {
            string text = File.ReadAllText( path );
            string[] words = text.Split( new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None );

            foreach (string word in words)
            {
                allWords.Add(word, 0);
            }
        }
        public void copyKeys(Dictionary<string, int> localDictionary)
        {
            lock (allWords)
            {
                foreach (KeyValuePair<string, int> single in allWords)
                {
                    localDictionary.Add(single.Key, 0);
                }
            }
        }
        public string makeCorrectedFileName(string baseName)
        {
            string[] parts = baseName.Split('.');
            parts[parts.Length - 2] = parts[parts.Length - 2] + "-corrected";
            string correctedLocalName = string.Join(".", parts);

            return replacePath(correctedLocalName);
        }

        public string replacePath(string basePath)
        {
            string[] parts = basePath.Split('\\');
            return directoryForCopy + @"\" + parts[parts.Length - 1];
        }
        private void startWritingReport()
        {
            using (StreamWriter writer = new StreamWriter(pathToReport, false))
            {
                writer.Write("----------");
                writer.Write(DateTime.Now);
                writer.WriteLine("----------");
            }
        }
        private void addFileToReport( Dictionary< string, int > localDictionary, FileInfo fileInfo )
        {
            using (StreamWriter writer = new StreamWriter(pathToReport, true))
            {
                writer.WriteLine(fileInfo.FullName);
                writer.WriteLine("File length: " + fileInfo.Length.ToString());
                writer.WriteLine("Words: ");
                lock (allWords)
                {
                    foreach (KeyValuePair<string, int> pair in localDictionary)
                    {
                        if (pair.Value != 0)
                        {
                            writer.WriteLine(pair.Key + " - " + pair.Value);
                        }
                    }
                }
            }
        }
        private void addFileToListView(Dictionary<string, int> localDictionary, FileInfo fileInfo)
        {
            window.addLineToListView(fileInfo.FullName);
        }
        private void endWritingReport()
        {
            List<KeyValuePair<string, int>> myList = null;

            lock (allWords)
            {
                myList = allWords.ToList();
            }

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
