namespace UltimateRedditBot.Core.Constants
{
    public class RedditApiConstants
    {
        public const string SearchSubRedditByNameUrl =
            "https://www.reddit.com/subreddits/search.json?q={0}&include_over_18=on&limit=1";

        public const string GetRedditPostBase = "https://reddit.com/r/{0}/{1}/.json?limit=10&{2}={3}";

        public const string IsPostRemovedUrl = "https://www.reddit.com{0}.json";
    }
}
