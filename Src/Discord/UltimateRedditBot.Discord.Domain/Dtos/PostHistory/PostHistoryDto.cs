using UltimateRedditBot.Domain.Dtos.Common;

namespace UltimateRedditBot.Discord.Domain.Dtos.PostHistory
{
    public class PostHistoryDto : BaseDto
    {
        #region Fields

        public int SubredditId { get; set; }

        public string PostId { get; set; }

        public ulong? UserId { get; set; }

        public ulong? GuildId { get; set; }

        #endregion
    }
}