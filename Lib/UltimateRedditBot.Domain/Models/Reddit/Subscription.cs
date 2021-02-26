using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class Subscription : BaseEntity
    {
        #region Properties

        public Sort Sort { get; set; }

        public string? PostId { get; set; }
        public Post Post { get; set; }

        public int SubredditId { get; set; }
        public Subreddit Subreddit { get; set; }

        #endregion
    }
}
