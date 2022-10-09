using System.Net;
using LeetCrawler.Downloader.Data;
using LeetCrawler.Downloader.Interfaces;
namespace LeetCrawler.Downloader
{
    public class SiteDownloader
    {
        private readonly HttpClient httpClient;
        private readonly ILinkExtractor linkExtractor;
        private readonly IDataStorage dataStorage;
        private readonly IProgress<ProgressDto> progressReporter;
        private List<string> downloadedLinks;
        public SiteDownloader(HttpClient httpClient,
                              ILinkExtractor linkExtractor,
                              IDataStorage dataStorage,
                              IProgress<ProgressDto> progressReporter)
        {
            this.httpClient = httpClient;
            this.linkExtractor = linkExtractor;
            this.dataStorage = dataStorage;
            this.progressReporter = progressReporter;
            this.downloadedLinks = new List<string>();
        }
        public async Task StartDownload(string baseUri, CancellationToken cancellationToken)
        {
            httpClient.BaseAddress = new Uri(baseUri);
            await RecursiveDownload(string.Empty, cancellationToken);
        }
        private async Task RecursiveDownload(string resourceToDownload, CancellationToken cancellationToken)
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            var pageContent = await DownloadPage(resourceToDownload, cancellationToken);
            downloadedLinks.Add(resourceToDownload);
            if (pageContent != null)
            {
                var savedFile = await this.dataStorage.SaveContent(resourceToDownload, pageContent, httpClient.BaseAddress!, cancellationToken);
                var extractedLinks = this.linkExtractor.ExtractLinks(pageContent);
                var newLinks = extractedLinks.Where(i => !downloadedLinks.Contains(i));
                stopWatch.Stop();
                var progressUpdate = new ProgressDto()
                {
                    SavedFile = savedFile,
                    ExtractedLinksNotYetSaved = newLinks.Count(),
                    TimeTakenToDownload = stopWatch.ElapsedMilliseconds
                };
                progressReporter.Report(progressUpdate);
                foreach (var link in newLinks)
                {
                    await RecursiveDownload(link, cancellationToken);
                }
            }
            return;
        }
        private async Task<string?> DownloadPage(string resourceToDownload, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(resourceToDownload, cancellationToken);
            switch (response?.StatusCode)
            {
                case HttpStatusCode.Moved:
                    var redirectUri = response.Headers?.Location;
                    if (redirectUri != null)
                        return await DownloadPage(redirectUri.AbsoluteUri);
                    return null;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return null;
            }
        }

    }
}