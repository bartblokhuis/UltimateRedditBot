using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class BannedSubreddit : BaseEntity
    {
        public int SubredditId { get; set; }

        public ulong? GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
