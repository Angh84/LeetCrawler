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

            IDataStorage dataStorage = new FileStorage(@"C:\Temp\leet\");

            var client = new HttpClient();

            var downloader = new SiteDownloader(client, linkExtractor, dataStorage);
            await downloader.StartDownload("https://www.tretton37.com");

            Console.ReadLine();
        }
    }
}