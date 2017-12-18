using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GoComics
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo(GlobalVars.DefaultCulture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            ComicsDb _dbComics = new ComicsDb();

            List<Comics> _lstComics = _dbComics.Select(null);
            DateTime forDay = DateTime.Today;

            JobDetailsDB _jobDetailsDb = new JobDetailsDB();
            JobDetails _jobDetails = new JobDetails
            {
                JobId = Guid.NewGuid(),
                StartTime = DateTime.Now
            };
            _jobDetailsDb.Insert(_jobDetails);

            foreach (Comics comic in _lstComics)
            {
                Console.WriteLine($"Getting {CreateUrl(comic, forDay)}");

                GetImagePath(comic, _jobDetails, forDay);
                //Job($"{comic.UrlComic}/{forDay}");

                Console.WriteLine($"Finished {comic.UrlComic}/{forDay}");
            }

            _jobDetails.EndTime = DateTime.Now;
            _jobDetailsDb.Update(_jobDetails);
        }

        private static void GetImagePath(Comics comic, JobDetails jDetails, DateTime forDay)
        {
            ComicsImg _comicsImg = new ComicsImg();

            _comicsImg.JobId = jDetails.JobId;
            _comicsImg.ComicId = comic.IdComic;

            _comicsImg.ForDate = forDay;
            string urlToGet = CreateUrl(comic, forDay);
            _comicsImg.ComicUrl = urlToGet;

            var task = MakeAsyncRequest(urlToGet, "text/html");

            var scrubbedHtml = ScrubbedHtml(task);

            Uri link = FetchLinksFromSource(scrubbedHtml);
            _comicsImg.ImgUrl = link.ToString();

            _comicsImg.ImagePath = SafeImage(link.ToString());

            Console.WriteLine(link.ToString());

            _comicsImg.Visited = DateTime.Now;

            ComicsImgDb _comicsImgDb = new ComicsImgDb();
            _comicsImgDb.Insert(_comicsImg);
        }

        private static string ScrubbedHtml(Task<string> task)
        {
            string htmlContent = task.Result;
            int startIndex = htmlContent.IndexOf("<div class=\"comic__image js-comic-swipe");
            int endIndex = htmlContent.IndexOf("<div class=\'comic__nav\'>");
            int _len = endIndex - startIndex;
            string scrubbedHtml = task.Result.Substring(startIndex, _len);

            startIndex = scrubbedHtml.IndexOf("<a");
            endIndex = scrubbedHtml.IndexOf("</a>");
            _len = endIndex - startIndex;
            scrubbedHtml = scrubbedHtml.Substring(startIndex, _len);
            return scrubbedHtml;
        }

        private static string CreateUrl(Comics comic, DateTime forDate)
        {
            return $"{comic.UrlComic}/{forDate.ToString("yyyy/MM/dd")}";
        }

        public static string SafeImage(string uri)
        {
            WebClient webClient = new WebClient();

            string _imgName = uri.Split('/')[uri.Split('/').Length - 1];
            string _imgPath = Environment.CurrentDirectory + "\\" + _imgName + ".png";

            webClient.DownloadFileAsync(new Uri(uri), _imgPath);

            return _imgPath;
        }

        public static Uri FetchLinksFromSource(string htmlSource)
        {
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                return new Uri(href);
            }
            return null;
        }

        // Define other methods and classes here
        public static Task<string> MakeAsyncRequest(string url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 20000;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }
    }
}
