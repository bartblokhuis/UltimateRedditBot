using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class SharedQueueModule : SharedCommandModule
    {
        #region Fields

        private readonly ISubredditService _subredditService;
        private readonly IPostHistoryService _postHistoryService;

        #endregion

        #region Constructor

        public SharedQueueModule(ISubredditService subredditService, IPostHistoryService postHistoryService)
        {
            _subredditService = subredditService;
            _postHistoryService = postHistoryService;
        }

        #endregion

        #region Methods

        #region Reset post history

        [Command("reset")]
        [Alias("reset")]
        public async Task ResetPostHistory(string subredditName)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            if (IsForGuild())
                await _postHistoryService.ClearPostHistory(true, Context.Guild.Id, subreddit.Id);
            else
                await _postHistoryService.ClearPostHistory(false, Context.User.Id, subreddit.Id);

            await ReplyAsync("Cleared the history");
        }

        #endregion

        #endregion
    }
}
