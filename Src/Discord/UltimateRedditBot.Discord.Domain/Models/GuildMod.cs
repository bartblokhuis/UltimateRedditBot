using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class GuildMod : BaseEntity
    {
        #region Properties

        public ulong GuildId { get; set; }
        public Guild Guild { get; set; }

        public ulong UserId { get; set; }
        public User User { get; set; }

        #endregion
    }
}
