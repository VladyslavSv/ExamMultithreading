using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace WrongWords
{
    public class FileSystemParser
    {
        private Dictionary<string, int> allWords = new Dictionary<string, int>();
        private string swapPattern = "*******";

        public FileSystemParser() { }

        public FileSystemParser(string pathToFileWithKeyWords)
        {
            initKeyWords(pathToFileWithKeyWords);
        }

        public void parseFiles(string path)
        {
            Dictionary<string, int> localDictionary = new Dictionary<string, int>();
            this.copyKeys(localDictionary);

            string[] allLines = new string[0];
            Regex regex = null;

            path += @"\";

            try
            {
                foreach (FileInfo innerInfo in new DirectoryInfo(path).GetFiles())
                {
                    //получим все строки файла
                    allLines = File.ReadAllLines(innerInfo.FullName);
                    //пройдемся по всем строкам файла
                    for (int i = 0; i < allLines.Length; i++)
                    {
                        //заменим все вхождения слова в строке
                        foreach (KeyValuePair<string, int> keyValue in allWords)
                        {
                            while (allLines[i].IndexOf(keyValue.Key) != -1)
                            {
                                regex = new Regex(keyValue.Key);
                                allLines[i] = regex.Replace(allLines[i], swapPattern, 1);

                                localDictionary[keyValue.Key]++;
                            }
                        }

                    }
                }

                foreach (DirectoryInfo directoryInfo in new DirectoryInfo(path).GetDirectories())
                {
                    parseFiles(directoryInfo.FullName);
                }
            }
            catch (UnauthorizedAccessException unauthorizedException)
            {

            }
            catch (IOException ex)
            {

            }

        }

        public void initKeyWords(string path)
        {
            string text = File.ReadAllText( path );
            string[] words = text.Split( ' ' );

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

    }
}
