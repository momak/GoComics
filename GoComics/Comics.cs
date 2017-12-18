using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace GoComics
{
    class Comics
    {
        private int _idComic;
        private string _urlComic;
        private string _name;
        private string _description;

        public int IdComic
        {
            get { return _idComic; }
            set { _idComic = value; }
        }

        public string UrlComic
        {
            get { return _urlComic; }
            set { _urlComic = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Comics()
        {
        }

        public Comics(int idComic, string urlComic, string name, string description)
        {
            IdComic = idComic;
            UrlComic = urlComic;
            Name = name;
            Description = description;

        }

        public override string ToString()
        {
            return $"{IdComic}, {UrlComic}, {Name}, {Description}";
        }
    }
}
