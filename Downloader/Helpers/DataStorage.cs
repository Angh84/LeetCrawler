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
        public async Task SaveContent(string downloadedSite, string content, Uri baseAddress)
        {
            var fullPath = ParseFileName(downloadedSite, baseAddress);
            var directory = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directory);
            using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

        }
        private string ParseFileName(string link, Uri? baseAddress)
        {
            string result = string.Empty;
            var parsedUri = ParseToUri(link, baseAddress);
            if (parsedUri != null)
            {
                var filename = SetWindowsStyleSeparator(string.Format("{0}{1}",parsedUri.Host, parsedUri.LocalPath));
                result = Path.Combine(rootFolder, filename);
            }
            if (!Path.HasExtension(result))
                result += @"\root.file";
            return result;
        }

        private string SetWindowsStyleSeparator(string path)
        {
            return path.Replace('/', '\\');
        }

        private Uri? ParseToUri(string link, Uri? baseAddress)
        {
            Uri? result;
            if (Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out result))
            {
                if (!result.IsAbsoluteUri)
                    Uri.TryCreate(baseAddress, result, out result);
            }
            return result;
        }
    }
}