using System.Net;
using LeetCrawler.Downloader.Interfaces;
namespace LeetCrawler.Downloader
{
    public class SiteDownloader
    {
        private readonly HttpClient httpClient;
        private readonly ILinkExtractor linkExtractor;
        private readonly IDataStorage dataStorage;
        private List<string> downloadedLinks;
        public SiteDownloader(HttpClient httpClient,
                              ILinkExtractor linkExtractor,
                              IDataStorage dataStorage)
        {
            this.httpClient = httpClient;
            this.linkExtractor = linkExtractor;
            this.dataStorage = dataStorage;
            this.downloadedLinks = new List<string>();
        }
        public async Task StartDownload(string baseUri)
        {
            httpClient.BaseAddress = new Uri(baseUri);
            await RecursiveDownload(string.Empty);
        }
        private async Task RecursiveDownload(string resourceToDownload)
        {
            var pageContent = await DownloadPage(resourceToDownload);
            downloadedLinks.Add(resourceToDownload);
            if (pageContent != null)
            {
                await this.dataStorage.SaveContent(resourceToDownload, pageContent, httpClient.BaseAddress);
                var extractedLinks = this.linkExtractor.ExtractLinks(pageContent);
                var newLinks = extractedLinks.Where(i => !downloadedLinks.Contains(i));
                foreach (var link in newLinks)
                {
                    await RecursiveDownload(link);
                }
            }
            return;
        }
        private async Task<string?> DownloadPage(string resourceToDownload)
        {
            var response = await httpClient.GetAsync(resourceToDownload);
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