using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class Subscription : BaseEntity
    {
        #region Properties

        public Sort Sort { get; set; }

        public string LastPostId { get; set; }

        public ulong SubredditId { get; set; }
        public Subreddit Subreddit { get; set; }

        #endregion
    }
}
