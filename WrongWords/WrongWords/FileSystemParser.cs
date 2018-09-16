using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
namespace WrongWords
{
    public class FileSystemParser
    {
        private Dictionary<string, int> allWords = new Dictionary<string, int>();
        private string swapPattern = "*******";
        private string pathToReport = "report.txt";
        public string directoryForCopy
        {
            get;set;
        }

        public FileSystemParser() { }

        public FileSystemParser(string pathToFileWithKeyWords, string directoryForCopy)
        {
            initKeyWords(pathToFileWithKeyWords);

            this.directoryForCopy = directoryForCopy;
        }


        public void parseFiles()
        {
            string scanPath = @"D:\MyTesting";

            startWritingReport();

            parseDirectory(scanPath);

            endWritingReport();
        }
        public void parseDirectory(string path)
        {
            Dictionary<string, int> localDictionary = null;
            string[] allLines = new string[0];
            Regex regex = null;
            StreamWriter writer = null;
            string newFileName = "";
            int replaceIndex = 0;
            path += @"\";

            try
            {

                //пройдемся по папкам в текущей директории
                foreach (FileInfo innerInfo in new DirectoryInfo(path).GetFiles())
                {
                    localDictionary = new Dictionary<string, int>();
                    this.copyKeys(localDictionary);
                    //получим все строки файла
                    allLines = File.ReadAllLines(innerInfo.FullName);
                   
                    //пройдемся по всем строкам файла
                    for (int i = 0; i < allLines.Length; i++)
                    {
                        //заменим все вхождения каждого слова в строке
                        foreach (KeyValuePair<string, int> keyValue in allWords)
                        {
                            //заменем пока встречается слово
                            while (allLines[i].IndexOf(keyValue.Key) != -1)
                            {
                                replaceIndex++;
                                if (replaceIndex == 1)
                                {
                                    newFileName = makeCorrectedFileName(innerInfo.FullName);
                                    writer = new StreamWriter(newFileName);
                                }

                                regex = new Regex(keyValue.Key);
                                allLines[i] = regex.Replace(allLines[i], swapPattern, 1);

                                localDictionary[keyValue.Key]++;
                            }
                        }

                        if (replaceIndex != 0)
                        {
                            writer.Write(allLines[i]);
                        }

                    }

                    if ( replaceIndex >= 1)
                    {
                        File.Copy( innerInfo.FullName, replacePath(innerInfo.FullName), true );

                        replaceIndex = 0;

                        writer.Flush();
                        writer.Close();

                        //добавим встречающиеся слова в общий зачет
                        foreach (KeyValuePair<string, int> pair in localDictionary)
                        {
                            allWords[pair.Key] += localDictionary[pair.Key];
                        }
                        //добавим запись в отчет
                        addFileToReport(localDictionary, innerInfo);
                    }
                }

                foreach (DirectoryInfo directoryInfo in new DirectoryInfo(path).GetDirectories())
                {
                    parseDirectory(directoryInfo.FullName);
                }

            }
            catch (UnauthorizedAccessException unauthorizedException)
            {
                System.Windows.MessageBox.Show(unauthorizedException.Message);
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

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
            foreach (KeyValuePair<string, int> single in allWords)
            {
                localDictionary.Add(single.Key, 0);
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

                foreach (KeyValuePair<string, int> pair in localDictionary)
                {
                    if (pair.Value != 0)
                    {
                        writer.WriteLine(pair.Key + " - " + pair.Value);
                    }
                }
            }
        }
        private void endWritingReport()
        {
            var myList = allWords.ToList();

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
