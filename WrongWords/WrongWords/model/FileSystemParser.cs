using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Threading;
using WrongWords.model;

namespace WrongWords
{
    public delegate void EmptyDelegate();
    public delegate void ListViewReporter(FileInfo fileInfo);

    public class FileSystemParser
    {
        public Dictionary<string, int> allWords = new Dictionary<string, int>();

        private ReportWriter reportWriter;

        public event EmptyDelegate couterChanged;
        public event ListViewReporter reporter;

        private string swapPattern = "*******";

        private volatile int wordsReplaces = 0;

        public int WordsReplaced
        {
            get
            {
                return wordsReplaces;
            }
            set
            {
                    wordsReplaces = value;    
                    couterChanged.Invoke();      
            }
        }

        public string directoryForCopy
        {
            get;set;
        }

        public FileSystemParser()
        {
            reportWriter = new ReportWriter("report.txt");
        }


        public async void parseFiles()
        {
            await Task.Run(() => {
                reportWriter.startWritingReport();

                List<Task> parseTasks = new List<Task>();

                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    parseTasks.Add(parseDirectory(driveInfo.Name));
                }
                Task.WaitAll(parseTasks.ToArray());
                MessageBox.Show("Сканирование файлов окончено.");
                reportWriter.endWritingReport(allWords);
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
                                                newFileName = PathHelper.makeCorrectedFileName(innerInfo.FullName, directoryForCopy);
                                                writer = new StreamWriter(newFileName);
                                            }
                                        }
                                        localDictionary[keyValue.Key]++;

                                       WordsReplaced++;
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
                            File.Copy(innerInfo.FullName, PathHelper.replacePath(innerInfo.FullName,directoryForCopy), true);

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
                           reporter.Invoke(innerInfo);
                           reportWriter.addFileToReport(localDictionary, innerInfo);
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
    }
}
