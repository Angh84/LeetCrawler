using System.Text;

namespace LeetCrawler.Downloader.Interfaces
{
    public interface IDataStorage
    {
        public Task<string> SaveContent(string relativePath, string content, Uri baseAddress, CancellationToken cancellationToken);
    }
   
}