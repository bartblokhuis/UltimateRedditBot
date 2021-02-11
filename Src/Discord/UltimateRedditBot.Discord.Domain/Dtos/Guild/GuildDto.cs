using System;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Dtos.Common;

namespace UltimateRedditBot.Discord.Domain.Dtos
{
    public class GuildDto : BaseDto<ulong>
    {
        public string Prefix { get; set; }

        public virtual Guild Guild { get; set; }
        public ulong GuildId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}