using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace WordDocMaker
{
    public class Nosi
    {
        public string Head { get; set; }
        public string Content { get; set; }
        public DateTime Publish { get; set; }
        public string Date { get; set; }
        public string Source { get; set; }
        public string SourceLink { get; set; }

        public static async Task GetSubNosi(string s)
        {
            IHtmlDocument doc = await Scraper.ScrapeWebsite(s, -1);
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
            Scraper.NosiList.Add(obj);
        }
        public static async Task GetNosiScraped()
        {
            Console.WriteLine("On page: " + Scraper.iPage);
            IHtmlDocument document = await Scraper.ScrapeWebsite(Scraper.siteUrlBase, Scraper.iPage);
            IEnumerable<IElement> articleLink;
            string temp = "";
            articleLink = document.All.Where(x => x.ClassName == "entry-title");
            List<String> NosiLinks = new List<String>();
            if (articleLink.Any())
            {
                foreach (IElement i in articleLink)
                {
                    IElement g = i.FirstElementChild;
                    if (!String.IsNullOrEmpty(g.GetAttribute("href")))
                    {
                        temp = g.GetAttribute("href");
                        NosiLinks.Add(temp);
                    }

                }
            }
            Scraper.iPage += 1;
            if (Scraper.iPage < 25)
            {
                await GetSubNosi(NosiLinks[Scraper.iPage - 1]);
                await GetNosiScraped();

            }
        }
    }

}
