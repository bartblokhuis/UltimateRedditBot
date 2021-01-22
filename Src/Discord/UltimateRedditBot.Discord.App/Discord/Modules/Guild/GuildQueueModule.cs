using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.Queue;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildQueueModule : UltimateGuildModule
    {
        #region Fields

        private readonly IQueueService _queueService;

        #endregion

        #region Constructor

        public GuildQueueModule(IQueueService queueService)
        {
            _queueService = queueService;
        }

        #endregion

        #region Methods

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingGuildGroup,
                ClientId = Context.Guild.Id,
                ChannelId = Context.Channel.Id
            };
            var result = await _queueService.AddToQueue(options, subreddit, 1);
            await ReplyAsync(result);
        }

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit, int amountOfTimes)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingGuildGroup,
                ClientId = Context.Guild.Id,
                ChannelId = Context.Channel.Id
            };
            var result = await _queueService.AddToQueue(options, subreddit, amountOfTimes);
            await ReplyAsync(result);
        }

        [Command("r-remove"), Alias("r-remove")]
        public async Task RemoveFromQueue(string subreddit)
        {
        }

        [Command("r-clear"), Alias("r-clear")]
        public async Task ClearQueue()
        {
        }

        #endregion
    }
}
