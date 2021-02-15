using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services.TextChannelService
{
    public interface ITextChannelService
    {
        Task<TextChannel> GetTextChannelById(ulong id);

        Task RegisterTextChannel(ulong id, ulong? userId, ulong? guilId);
    }
}
