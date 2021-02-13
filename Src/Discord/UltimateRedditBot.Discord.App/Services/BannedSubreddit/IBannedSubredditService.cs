using System.Threading.Tasks;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IBannedSubredditService
    {
        Task<string> BanSubreddit(ulong id, string subredditName);

        Task<bool> IsSubredditBanned(ulong id, string subredditName);

        Task<string> UnbanSubreddit(ulong id, string subredditName);
    }
}
