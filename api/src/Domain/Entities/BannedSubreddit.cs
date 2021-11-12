using Domain.Entities.Common;

namespace Domain.Entities
{
    public class BannedSubreddit : BaseEntity
    {
        #region Properties

        public Guild Guild { get; set; }

        public Guid GuildId { get; set; }

        public Subreddit Subreddit { get; set; }

        public Guid SubredditId { get; set; }

        #endregion
    }
}
