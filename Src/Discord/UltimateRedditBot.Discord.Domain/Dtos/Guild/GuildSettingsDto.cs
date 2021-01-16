using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Dtos.Common;

namespace UltimateRedditBot.Discord.Domain.Dtos
{
    public class GuildSettingsDto : BaseDto
    {
        public GuildSettingsDto()
        {

        }

        public GuildSettingsDto(ulong guildId)
        {
            GuildId = guildId;
        }

        public string Prefix { get; set; }

        public virtual Guild Guild { get; set; }
        public ulong GuildId { get; set; }
    }
}
