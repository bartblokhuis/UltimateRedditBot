using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services.Guild
{
    public interface IGuildService
    {
        Task InsertGuild(GuildDto guildDto);

        Task RegisterNewGuilds(IEnumerable<GuildDto> guilds);

        Task<Domain.Models.Guild> GetById(ulong guildId);

        GuildSettings GetGuildSettingsById(ulong guildId);

        Task SaveGuildSettings(GuildSettings guildSettingsDto);

        string GetPrefix(ulong guildId);
    }
}
