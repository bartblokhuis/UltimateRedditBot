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
        private readonly IGuildModService _guildModService;

        #endregion

        #region Constructor

        public BanSubredditModule(IBannedSubredditService bannedSubredditService, IGuildModService guildModService)
        {
            _bannedSubredditService = bannedSubredditService;
            _guildModService = guildModService;
        }

        #endregion

        #region Commands

        [Command("ban")]
        [Alias("b")]
        public async Task BanSubreddit(string subreddit)
        {
            if (!IsMod())
            {
                await ReplyAsync("Only bot mods can manage subreddit bans");
                return;
            }

            var result = await _bannedSubredditService.BanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }


        [Command("unban")]
        [Alias("ub")]
        public async Task UnbanSubreddit(string subreddit)
        {
            if (!IsMod())
            {
                await ReplyAsync("Only bot mods can manage subreddit bans");
                return;
            }

            var result = await _bannedSubredditService.UnbanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }

        [Command("bans")]
        public async Task Bans()
        {
            await ReplyAsync("Coming soon");
        }

        #endregion

        #region utils

        private bool IsMod()
        {
            return _guildModService.IsMod(Context.User.Id, Context.Guild.Id);
        }

        #endregion
    }
}
