using System;
using UltimateRedditBot.Domain.Dtos.Common;

namespace UltimateRedditBot.Discord.Domain.Dtos
{
    public class GuildDto : BaseDto<ulong>
    {
        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
