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
using WordDocMaker;

namespace WordDocMaker
{
    public static class Scraper
    {
        public static string siteUrlBase = "https://nosi.org/page/";
        public static int iPage = 0;
        public static List<Nosi> NosiList = new List<Nosi>();
        public static async Task<IHtmlDocument> ScrapeWebsite(string siteurl,int page)
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
    }


}
