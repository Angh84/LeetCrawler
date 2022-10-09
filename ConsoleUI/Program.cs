using LeetCrawler.Downloader;
using LeetCrawler.Downloader.Helpers;
using LeetCrawler.Downloader.Interfaces;

namespace LeetCrawler.ConsoleUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var extractionPattern = @"href=[\'""]?([^\'"" >]+)";
            ILinkExtractor linkExtractor = new RegExLinkExtractor(extractionPattern);
            linkExtractor.AddLinkFilter(ExtractionRules.NoEmptyLinks);
            linkExtractor.AddLinkFilter(ExtractionRules.NoSamePageRedirects);
            linkExtractor.AddLinkFilter(ExtractionRules.DisallowPath("/assets"));
            linkExtractor.AddLinkFilter(ExtractionRules.DisallowPath("assets"));
            linkExtractor.AddLinkFilter(ExtractionRules.AllowHosts(new List<string>() { "www.tretton37.com", "tretton37.com" }));

            IDataStorage dataStorage = new FileStorage(@"C:\Temp\leet\");

            var client = new HttpClient();

            var downloader = new SiteDownloader(client, linkExtractor, dataStorage);
            await downloader.StartDownload("https://www.tretton37.com");

            Console.ReadLine();
        }
    }
}