using LeetCrawler.Downloader.Interfaces;
namespace LeetCrawler.Downloader.Helpers
{
    public class FileStorage : IDataStorage
    {
        private string rootFolder;
        private int fileCounter;

        public FileStorage(string rootFolder)
        {
            if (!Directory.Exists(rootFolder))
                Directory.CreateDirectory(rootFolder);

            this.rootFolder = rootFolder;
        }
        public async Task SaveContent(string downloadedSite, string content)
        {
            var fileName = getFileName();
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

        }
        private string getFileName()
        {
            string result = rootFolder;
            result += (fileCounter++).ToString();
            result += ".txt";
            return result;
        }
    }
}