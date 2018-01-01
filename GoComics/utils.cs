using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoComics
{
    public static class utils
    {
        public static bool CreateFolder(string pathToFolder)
        {
            if (!string.IsNullOrWhiteSpace(pathToFolder))
            {
                if (!Directory.Exists(pathToFolder))
                    Directory.CreateDirectory(pathToFolder);

                return true;
            }
            return false;
        }

        public static string CreateFile(string pathToFile)
        {
            if (!string.IsNullOrWhiteSpace(pathToFile))
            {

                string outputFile = pathToFile + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm") + ".txt";

                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                return outputFile;
            }

            throw new Exception("Cannot create output directory/file!");
        }

    }
}
