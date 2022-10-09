namespace LeetCrawler.Downloader.Helpers
{
    public static class ExtractionRules
    {
        public static bool NoEmptyLinks(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
                return false;
            return link != "/";
        }

        public static bool NoSamePageRedirects(string link)
        {
            return !link.StartsWith("#");
        }
        public static Predicate<string> DisallowPath(string path)
        {
            Predicate<string> rule = new Predicate<string>((link) =>
            {
                return !link.StartsWith(path);
            });
            return rule;
        }
        public static Predicate<string> AllowHosts(List<string> hosts)
        {
            Predicate<string> rule = new Predicate<string>((link) =>
            {
                if (Uri.TryCreate(link,UriKind.RelativeOrAbsolute, out var uri))
                {
                    if (uri.IsAbsoluteUri)
                        return hosts.Contains(uri.Host);
                    return true;
                }
                return false;
            });
            return rule;
        }
        public static Predicate<string> CantContainString(string disallowedString)
        {
            Predicate<string> rule = new Predicate<string>((link) =>
            {
                return !link.Contains(disallowedString);
            });
            return rule;
        }
    }
}