using System.Text.RegularExpressions;
using LeetCrawler.Downloader.Interfaces;

namespace LeetCrawler.Downloader.Helpers
{
    public class RegExLinkExtractor : ILinkExtractor
    {
        private readonly string ExtractionPattern;
        public RegExLinkExtractor(string regExPattern)
        {
            ExtractionPattern = regExPattern;
        }
        public List<string> ExtractLinks(string content)
        {
            var result = new List<string>();

            foreach (Match match in Regex.Matches(content, ExtractionPattern, RegexOptions.IgnoreCase))
            {
                if (match.Groups.Count < 2)
                    continue;

                var link = match.Groups[1].Value;
                if (link.StartsWith("/assets") || link.StartsWith("assets") || result.Contains(link))
                    continue;

                result.Add(link);
            }
            return result;
        }
    }
}