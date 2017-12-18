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

       }
}
