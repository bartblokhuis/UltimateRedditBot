using System.Threading.Tasks;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IGuildModService
    {
        Task<string> Mod(ulong userId, ulong guildIds);

        bool IsMod(ulong userId, ulong guildIds);

        Task<string> UnMod(ulong userId, ulong guildIds);
    }
}
