using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WrongWords.model
{
    static class PathHelper
    {
        public static string makeCorrectedFileName(string baseName, string directoryForCopy)
        {
            string[] parts = baseName.Split('.');
            parts[parts.Length - 2] = parts[parts.Length - 2] + "-corrected";
            string correctedLocalName = string.Join(".", parts);

            return replacePath(correctedLocalName, directoryForCopy);
        }

        public static string replacePath(string basePath, string directoryForCopy)
        {
            string[] parts = basePath.Split('\\');
            return directoryForCopy + @"\" + parts[parts.Length - 1];
        }
        public static bool isPathRoot(string path)
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
    }
}
