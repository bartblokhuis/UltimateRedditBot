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

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        //public virtual ICollection<SubRedditHistory> SubRedditHistories { get; set; }
        public ICollection<DiscordChannel> Channels { get; set; }

        #endregion

    }
}
