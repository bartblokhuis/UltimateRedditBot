using Domain.Entities.Common;

namespace Domain.Entities
{
    public class ChannelSubscription : BaseEntity
    {
        #region Properties

        public Subscription Subscription { get; set; }

        public Guid SubscriptionId { get; set; }

        public GuildChannel GuildChannel { get; set; }

        public Guid GuildChannelId { get; set; }

        #endregion
    }
}
