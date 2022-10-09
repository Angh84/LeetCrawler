using LeetCrawler.Downloader;
using LeetCrawler.Downloader.Data;
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
            var cts = new CancellationTokenSource();
            var client = new HttpClient();
            var progress = new Progress<ProgressDto>((val) =>
            {
                var output = string.Format("File: {0} downloaded in {1}ms, {2} new links added to queue", val.SavedFile, val.TimeTakenToDownload, val.ExtractedLinksNotYetSaved);
                Console.WriteLine(output);
            });
            var downloader = new SiteDownloader(client, linkExtractor, dataStorage, progress);
            Console.CancelKeyPress += (s, e) =>
            {
                cts.Cancel();
                e.Cancel = true;
            };
            Console.WriteLine("Download starting");
            try
            {
                await downloader.StartDownload("https://www.tretton37.com", cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Download canceled");
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error, shutting down.");
            }


        }
    }
}