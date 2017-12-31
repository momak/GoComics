using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoComics
{
    public static class GlobalVars
    {
        public static string DefaultCulture => ConfigurationManager.AppSettings["DefaultCulture"];
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];
        public static string ComicsFolder => ConfigurationManager.AppSettings["comicsFolder"];
        public static int DaysBefore => Convert.ToInt32(ConfigurationManager.AppSettings["getDaysBefore"]);
    }
}
