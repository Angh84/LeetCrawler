namespace LeetCrawler.Downloader.Interfaces
{
    public interface ILinkExtractor
    {
        public void AddLinkFilter(Predicate<string> filter);

        public List<string> ExtractLinks(string content);
    }

}