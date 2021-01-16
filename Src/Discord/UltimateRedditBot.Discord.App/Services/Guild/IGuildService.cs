using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IGuildService
    {
        Task InsertGuild(GuildDto guildDto);

        Task RegisterNewGuilds(IEnumerable<GuildDto> guilds);

        Task<Guild> GetById(ulong guildId);

        Task<GuildSettingsDto> GetGuildSettingsById(ulong guildId);

        Task SaveGuildSettings(GuildSettingsDto guildSettingsDto);
    }
}
