using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class TextChannel : BaseEntity
    {
        public ulong TextChannelId { get; set; }

        public ulong? UserId { get; set; }
        public User User { get; set; }

        public ulong? GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
