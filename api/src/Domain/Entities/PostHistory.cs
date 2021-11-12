using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class PostHistory : BaseEntity
    {
        #region Properties

        public string LastPostId { get; set; }

        public Sort Sort { get; set; }

        public Subreddit Subreddit { get; set; }

        public Guid SubredditId { get; set; }

        public Guild Guild { get; set; }

        public Guid GuildId { get; set; }

        #endregion
    }
}
