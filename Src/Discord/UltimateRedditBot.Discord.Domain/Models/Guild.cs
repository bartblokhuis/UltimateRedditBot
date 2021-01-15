using System;
using System.Collections.Generic;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class Guild : BaseEntity<ulong>, IFullyAuditedObject
    {
        #region Contructor

        //Empty ctor for ef core
        public Guild()
        { }

        public Guild(ulong guildId)
        {
            Id = guildId;
            Channels = new List<DiscordChannel>();
        }

        #endregion

        #region Properties

        public DateTime UpdatedAtUTC { get; set; }

        public DateTime CreatedAtUTC { get; set; }

        public virtual ICollection<DiscordChannel> Channels { get; set; }

        #endregion

    }
}
