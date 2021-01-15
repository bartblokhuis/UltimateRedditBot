using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IGuildService
    {
        Task AddGuild(GuildDto guildDto);
    }
}
