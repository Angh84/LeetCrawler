namespace LeetCrawler.Downloader.Data
{
    public class ProgressDto
    {
        public string SavedFile { get; set; } = string.Empty;
        public long TimeTakenToDownload { get; set; }
        public int ExtractedLinksNotYetSaved { get; set; }
    }
   
}