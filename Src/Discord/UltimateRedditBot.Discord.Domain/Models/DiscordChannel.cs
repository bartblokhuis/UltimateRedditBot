using System;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class DiscordChannel : BaseEntity<ulong>
    {
        #region Constructor

        //Empty ctor for ef core
        public DiscordChannel()
        {
            //Subscriptions = new List<Subscription>();
        }

        public DiscordChannel(ulong channelId, ulong guildId)
        {
            Id = channelId;
            GuildId = guildId;
            //Subscriptions = new List<Subscription>();
        }

        #endregion

        #region Properties

        public virtual Guild Guild { get; set; }
        public ulong GuildId { get; set; }

        #endregion
    }
}
