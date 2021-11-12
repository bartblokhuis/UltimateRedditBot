using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Subscription : BaseEntity
    {
        #region Properties

        public string LastPostId { get; set; } = "";

        public Sort Sort { get; set; }

        public Subreddit Subreddit { get; set; }

        public Guid SubredditId { get; set; }

        public ICollection<ChannelSubscription> ChannelSubscriptions { get; set; }

        #endregion

        #region Constructor

        public Subscription()
        {
            ChannelSubscriptions = new HashSet<ChannelSubscription>();
        }

        #endregion
    }
}
