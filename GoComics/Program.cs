using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GoComics.DAL;
using GoComics.Models;

namespace GoComics
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo(GlobalVars.DefaultCulture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            ComicsManager _dbComics = new ComicsManager();

            List<Comics> _lstComics = _dbComics.Select(null);
            DateTime forDay = DateTime.Today.AddDays(-2);
            
            JobDetailsManager _jobManager = new JobDetailsManager();
            JobDetails _jobDetails = new JobDetails
            {
                JobId = Guid.NewGuid(),
                StartTime = DateTime.Now
            };
            _jobManager.Insert(_jobDetails);

            Parallel.ForEach(_lstComics, comic =>
            {
                Console.WriteLine($"Getting {CreateUrl(comic, forDay)}");

                GetImagePath(comic, _jobDetails, forDay);
                //Job($"{comic.UrlComic}/{forDay}");

                Console.WriteLine($"Finished {comic.UrlComic}/{forDay}");
            });
            

            _jobDetails.EndTime = DateTime.Now;
            _jobManager.Update(_jobDetails);
        }

        private static void GetImagePath(Comics comic, JobDetails jDetails, DateTime forDay)
        {
            ComicsImg comicsImg = new ComicsImg();

            comicsImg.JobId = jDetails.JobId;
            comicsImg.ComicId = comic.IdComic;

            comicsImg.ForDate = forDay;
            string urlToGet = CreateUrl(comic, forDay);
            comicsImg.ComicUrl = urlToGet;

            var task = MakeAsyncRequest(urlToGet, "text/html");

            var scrubbedHtml = ScrubbedHtml(task);

            Uri link = FetchLinksFromSource(scrubbedHtml);
            comicsImg.ImgUrl = link.ToString();

            if (!DownloadRemoteImageFile(link.ToString(), comicsImg, out var imagePath))
            {
                return;
            }
            comicsImg.ImagePath = imagePath;

            Console.WriteLine(link.ToString());

            comicsImg.Visited = DateTime.Now;

            ComicsImgManager comicsImgManager = new ComicsImgManager();
            comicsImgManager.Insert(comicsImg);
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

        public static string SaveImage(string uri, ComicsImg comicsImg)
        {
            //string imgName = comicsImg.ForDate.ToString("yyyy_MM_dd")+".png";
            string imgName = comicsImg.ComicId + ".png";
            string imgLocalPath = GlobalVars.RootFolder + GlobalVars.ComicsFolder + comicsImg.ComicId + "\\" + imgName + ".png";

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFileAsync(new Uri(uri), imgName);
                }
                catch
                {
                    return String.Empty;
                }

            }

            return imgLocalPath;
        }

        private static bool DownloadRemoteImageFile(string uri, ComicsImg comicsImg, out string imgLocalPath)
        {
            string imgName = comicsImg.ForDate.ToString("yyyy_MM_dd")+".png";
            utils.CreateFolder(GlobalVars.RootFolder + GlobalVars.ComicsFolder + comicsImg.ComicId);

            imgLocalPath = GlobalVars.RootFolder + GlobalVars.ComicsFolder + comicsImg.ComicId + "\\" + imgName;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download it
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(imgLocalPath))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
                return true;
            }
            return false;
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
