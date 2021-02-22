using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class TextChannelSubscription : BaseEntity
    {
        #region Properties

        public int TextChannelId { get; set; }
        public TextChannel TextChannel { get; set; }

        public int SubscriptionId { get; set; }

        #endregion
    }
}
