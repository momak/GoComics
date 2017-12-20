using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace GoComics.Models
{
    class ComicsImg
    {
        private int _idImg;
        private Guid _jobId;
        private int _comicId;

        /// <summary>
        /// image url path
        /// </summary>
        private string _imgUrl;
        /// <summary>
        /// comic physical path
        /// </summary>
        private string _imagePath;
        /// <summary>
        /// comic url path
        /// </summary>
        private string _comicUrl;
        private DateTime _forDate;
        private DateTime _visited;

        public int IdImg
        {
            get { return _idImg; }
            set { _idImg = value; }
        }
        public Guid JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }
        public int ComicId
        {
            get { return _comicId; }
            set { _comicId = value; }
        }
        public string ImgUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }
        public string ComicUrl
        {
            get { return _comicUrl; }
            set { _comicUrl = value; }
        }
        public DateTime ForDate
        {
            get { return _forDate; }
            set { _forDate = value; }
        }
        public DateTime Visited
        {
            get { return _visited; }
            set { _visited = value; }
        }

        public ComicsImg()
        {
        }

        public ComicsImg(int idImg, Guid jobId, int comicId, string imgUrl, string imagePath, string cosmicUrl, DateTime forDate, DateTime visited)
        {
            IdImg = idImg;
            JobId = jobId;
            ComicId = comicId;
            ImgUrl = imgUrl;
            ImagePath = imagePath;
            ComicUrl = cosmicUrl;
            ForDate = forDate;
            Visited = visited;

        }

        public override string ToString()
        {
            return $"{IdImg}, {ImgUrl}, {ImagePath}, {ComicUrl}, {ForDate.ToString("dd.MM.yyyy")}";
        }

    }
}
