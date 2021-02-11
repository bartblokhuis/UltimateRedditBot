using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Dtos
{
    public class UserSettingsDto : BaseEntity
    {
        public UserSettingsDto()
        {
        }

        public UserSettingsDto(ulong userId)
        {
            UserId = userId;
        }

        public User User { get; set; }
        public ulong UserId { get; set; }

        public string Prefix { get; set; }
    }
}