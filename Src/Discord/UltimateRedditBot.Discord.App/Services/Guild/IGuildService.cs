using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Services.Guild
{
    public interface IGuildService
    {
        Task InsertGuild(GuildDto guildDto);

        Task RegisterNewGuilds(IEnumerable<GuildDto> guilds);

        Task<Domain.Models.Guild> GetById(ulong guildId);

        Task<GuildSettingsDto> GetGuildSettingsById(ulong guildId);

        Task SaveGuildSettings(GuildSettingsDto guildSettingsDto);
    }
}
