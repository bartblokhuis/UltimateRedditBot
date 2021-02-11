using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class UserSettings : BaseEntity
    {
        public User User { get; set; }
        public ulong UserId { get; set; }

        public string Prefix { get; set; }
    }
}