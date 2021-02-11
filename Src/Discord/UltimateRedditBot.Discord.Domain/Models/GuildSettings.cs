using System;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class GuildSettings : BaseEntity, IFullyAuditedObject
    {
        public string Prefix { get; set; }

        public virtual Guild Guild { get; set; }
        public ulong GuildId { get; set; }

        public DateTime CreatedAtUTC { get; set; }
        public DateTime UpdatedAtUTC { get; set; }
    }
}