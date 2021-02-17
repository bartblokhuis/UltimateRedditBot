using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services.TextChannelService
{
    public interface ITextChannelService
    {
        Task<TextChannel> GetTextChannelById(ulong id, ulong? guildId, ulong? userId);

        Task<TextChannel> RegisterTextChannel(ulong id, ulong? guildId, ulong? userId);
    }
}
