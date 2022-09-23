using System;
namespace LeetCrawler.Downloader
{
    public class SiteDownloader
    {
        private string baseUrl;
        private string downloadPath;
        private readonly HttpClient httpClient;

        public SiteDownloader(string baseUrl, string downloadPath)
        {
            this.baseUrl = baseUrl;
            this.downloadPath = downloadPath;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<string> DownloadPage(string relativePath)
        {
            return await httpClient.GetStringAsync(relativePath);
        }
  
    }
}