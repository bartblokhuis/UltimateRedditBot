using Domain.Entities.Common;

namespace Domain.Entities
{
    public class GuildChannel : BaseEntity
    {
        #region Properties

        public string DiscordChannelId { get; set; }

        public Guild Guild { get; set; }

        public Guid GuildId { get; set; }

        public ICollection<ChannelSubscription> ChannelSubscriptions { get; set; }

        #endregion

        #region Constructor

        public GuildChannel()
        {
            ChannelSubscriptions = new HashSet<ChannelSubscription>();
        }

        #endregion
    }
}
