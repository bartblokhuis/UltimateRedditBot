using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Discord.Modules.Helpers;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.Queue;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildQueueModule : UltimateGuildModule
    {
        #region Fields

        private readonly IBannedSubredditService _bannedSubredditService;
        private readonly IQueueService _queueService;

        #endregion

        #region Constructor

        public GuildQueueModule(IQueueService queueService, IBannedSubredditService bannedSubredditService)
        {
            _queueService = queueService;
            _bannedSubredditService = bannedSubredditService;
        }

        #endregion

        #region Methods

        #region Add to queue

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subreddit)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingGuildGroup,
                ClientId = Context.Guild.Id,
                ChannelId = Context.Channel.Id
            };

            await AddToQueue(options, subreddit, 1);
        }

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subreddit, int amountOfTimes)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingGuildGroup,
                ClientId = Context.Guild.Id,
                ChannelId = Context.Channel.Id
            };

            await AddToQueue(options, subreddit, amountOfTimes);
        }

        private async Task AddToQueue(AddToQueueDiscordOptions options, string subredditName, int amountOfTimes)
        {
            if (await _bannedSubredditService.IsSubredditBanned(Context.Guild.Id, subredditName))
            {
                await ReplyAsync("This subreddit is banned");
                return;
            }

            var result = await _queueService.AddToQueue(options, subredditName, amountOfTimes);
            await ReplyAsync(result);
        }

        #endregion

        #region Get Queue

        [Command("queue")]
        [Alias("q")]
        public async Task GetQueue()
        {
            var options = new GetQueueDto
            {
                Id = Context.Guild.Id,
                ChannelId = Context.Channel.Id,
                 Group = DiscordSettings.GenericSettingGuildGroup
            };

            var queueClient =  _queueService.GetQueueClient(options);

            if (queueClient == null)
            {
                await ReplyAsync("No items in queue");
                return;
            }

            var queueMessageBuilder = new EmbedBuilder
            {
                Title = $"Queue items in {Context.Channel.Name}:",
                Fields = EmbedBuilderHelper.EmbedQueueItems(queueClient.QueueItems),
                Footer = EmbedBuilderHelper.QueueItemsFooter(queueClient.QueueItems.Select(x => x.AmountOfPosts).Sum())
            };

            await ReplyAsync("", false, queueMessageBuilder.Build());
        }

        #endregion


        [Command("r-remove")]
        [Alias("r-remove")]
        public async Task RemoveFromQueue(string subreddit)
        {
        }

        [Command("r-clear")]
        [Alias("r-clear")]
        public async Task ClearQueue()
        {
        }

        #endregion
    }
}
