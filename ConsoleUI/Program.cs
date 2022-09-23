using LeetCrawler.Downloader;

namespace LeetCrawler.ConsoleUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = @"https:\\tretton37.com";
            var downloadPath = @"C:\Temp\leet\";
            var downloader = new SiteDownloader(baseUrl,downloadPath);
            var downloadResult = await downloader.DownloadPage(string.Empty);
            if (downloadResult != null)
                Console.WriteLine(downloadResult);
            else
            Console.WriteLine("Download failed");
            Console.ReadLine();
        }
    }
}