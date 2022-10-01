using System.Text.RegularExpressions;
using LeetCrawler.Downloader.Interfaces;

namespace LeetCrawler.Downloader.Helpers
{
    public class RegExLinkExtractor : ILinkExtractor
    {
        private readonly string _extractionPattern;
        private List<Predicate<string>> _extractionRules { get; set; }
        public RegExLinkExtractor(string regExPattern)
        {
            _extractionPattern = regExPattern;
            _extractionRules = new List<Predicate<string>>();
            _extractionRules.Add((link) => { return true; });
        }

        public void AddLinkFilter(Predicate<string> filter)
        {
            _extractionRules.Add(filter);
        }

        public List<string> ExtractLinks(string content)
        {
            var result = new List<string>();

            foreach (Match match in Regex.Matches(content, _extractionPattern, RegexOptions.IgnoreCase))
            {
                if (match.Groups.Count < 2)
                    continue;

                var link = match.Groups[1].Value;

                if (result.Contains(link))
                    continue;

                result.Add(link);
            }
            return result.Where(i => _extractionRules.All(p => p(i))).ToList();
        }
    }
}