using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoComics
{
    public enum LogMode
    {
        Console,
        File,
        Both
    }

    public enum LogDetail
    {
        None, 
        Information,
        Error,
        Success,
        Debug
    }
}
