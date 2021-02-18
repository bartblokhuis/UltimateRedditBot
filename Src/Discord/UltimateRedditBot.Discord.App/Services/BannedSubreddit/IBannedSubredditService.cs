using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IBannedSubredditService
    {
        Task<string> BanSubreddit(ulong id, string subredditName);

        Task<bool> IsSubredditBanned(ulong id, string subredditName);

        bool IsSubredditBanned(ulong id, SubredditDto subreddit);

        Task<string> UnbanSubreddit(ulong id, string subredditName);

        Task<List<int>> GetBannedSubredditIds(ulong guildId);

        Task<IEnumerable<SubredditDto>> GetBannedSubreddits(ulong guildId);
    }
}
