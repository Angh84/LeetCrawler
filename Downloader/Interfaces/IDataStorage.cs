using System.Text;

namespace LeetCrawler.Downloader.Interfaces
{
    public interface IDataStorage
    {
        public Task SaveContent(string relativePath, string content);
    }
   
}