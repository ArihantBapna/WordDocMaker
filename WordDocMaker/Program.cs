using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using System.Text;
namespace ConsoleScraper
{
    class MainClass
    {
        private string Title { get; set; }
        private string Url { get; set; }
        private string siteUrlBase = "https://nosi.org/page/";
        private int iPage = 0;
        public static List<Nosi> NosiList = new List<Nosi>();
        internal async Task<IHtmlDocument> ScrapeWebsite(string siteurl,int page)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request;
            if(page != -1)
            {
                 request = await httpClient.GetAsync(siteurl + page.ToString() + "/");
            }
            else
            {
                 request = await httpClient.GetAsync(siteurl);
            }
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
            return document;
        }
        private async Task GetNosiScraped()
        {
            Console.WriteLine("On page: " + iPage);
            IHtmlDocument document =  await ScrapeWebsite(siteUrlBase, iPage);
            IEnumerable<IElement> articleLink;
            string temp = "";
            articleLink = document.All.Where(x => x.ClassName == "entry-title");
            List<String> NosiLinks = new List<String>();
            if (articleLink.Any())
            {
                foreach(IElement i in articleLink)
                {
                    IElement g = i.FirstElementChild;
                    if (!String.IsNullOrEmpty(g.GetAttribute("href")))
                    {
                        temp = g.GetAttribute("href");
                        NosiLinks.Add(temp);
                    }

                }
            }
            iPage += 1;
            if(iPage < 25)
            {
                await GetSubNosi(NosiLinks[iPage-1]);
                await GetNosiScraped();
                
            }
        }

        private async Task GetSubNosi(string s)
        {
            IHtmlDocument doc = await ScrapeWebsite(s, -1);
            Encoding unicode = new UnicodeEncoding();
            IEnumerable<IElement> header = doc.All.Where(x => x.ClassName == "entry-title");
            IEnumerable<IElement> content = doc.All.Where(x => x.ClassName == "entry-content");

            IElement h = header.First();
            string head = h.TextContent;
            head = head.Replace("\u00A0", " ");

            IElement c = content.First();
            IElement gChild = c.FirstElementChild;
            var cChild = gChild.Children;

            IElement src = cChild.First();
            string hrefSrc = src.GetAttribute("href");
            string source = src.TextContent;

            string completeContent = gChild.TextContent;
            completeContent = completeContent.Replace("\u00A0", " ");


            IEnumerable<IElement> dateList = doc.All.Where(x => x.ClassName == "entry-date");
            IElement date = dateList.First();

            string dnt = date.TextContent;
            DateTime dt = Convert.ToDateTime(date.GetAttribute("datetime"));

            Nosi obj = new Nosi();
            obj.Head = head;
            obj.Content = completeContent;
            obj.Publish = dt;
            obj.Date = dnt;
            obj.SourceLink = hrefSrc;
            obj.Source = source;
            Console.WriteLine(completeContent);
            NosiList.Add(obj);
        }
    }

    public class Nosi
    {
        public string Head { get; set; }
        public string Content { get; set; }
        public DateTime Publish { get; set; }
        public string Date { get; set; }
        public string Source { get; set; }
        public string SourceLink { get; set; }
    }
}
