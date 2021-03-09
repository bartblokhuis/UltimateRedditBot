using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class PostHistory : BaseEntity
    {
        #region Fields

        public int SubredditId { get; set; }

        public string PostId { get; set; }

        public ulong? UserId { get; set; }

        public ulong? GuildId { get; set; }

        public Sort Sort { get; set; }

        #endregion
    }
}