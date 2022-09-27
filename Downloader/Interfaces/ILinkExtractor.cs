namespace LeetCrawler.Downloader.Interfaces
{
    public interface ILinkExtractor
    {
        public List<string> ExtractLinks(string content);
    }

}