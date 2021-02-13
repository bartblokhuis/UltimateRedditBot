using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class BanSubredditModule : UltimateGuildModule
    {
        #region Fields

        private readonly IBannedSubredditService _bannedSubredditService;

        #endregion

        #region Constructor

        public BanSubredditModule(IBannedSubredditService bannedSubredditService)
        {
            _bannedSubredditService = bannedSubredditService;
        }

        #endregion

        #region Commands

        [Command("ban")]
        [Alias("b")]
        public async Task BanSubreddit(string subreddit)
        {
            var result = await _bannedSubredditService.BanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }


        [Command("unban")]
        [Alias("ub")]
        public async Task UnbanSubreddit(string subreddit)
        {
            var result = await _bannedSubredditService.UnbanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }

        #endregion
    }
}
