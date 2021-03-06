using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Discord.Modules.Helpers;
using UltimateRedditBot.Discord.App.Services.Queue;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessagesQueueModule : UltimateDirectMessageModule
    {
        #region Fields

        private readonly IQueueService _queueService;
        private readonly ISubredditService _subredditService;

        #endregion

        #region Constructor

        public DirectMessagesQueueModule(IQueueService queueService, ISubredditService subredditService)
        {
            _queueService = queueService;
            _subredditService = subredditService;
        }

        #endregion

        #region Methods

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subredditName)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingDmGroup,
                ClientId = Context.User.Id
            };

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            await _queueService.AddToQueue(options, subreddit, 1);
        }

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subredditName, int amountOfTimes)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingDmGroup,
                ClientId = Context.User.Id
            };

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            await _queueService.AddToQueue(options, subreddit, amountOfTimes);
        }

        #region Get Queue

        [Command("queue")]
        [Alias("q")]
        public async Task GetQueue()
        {
            var options = new QueueOptions
            {
                Id = Context.User.Id,
                Group = DiscordSettings.GenericSettingDmGroup
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

        [Command("q-remove")]
        [Alias("q-remove")]
        public async Task RemoveFromQueue(string subreddit)
        {
            var options = new QueueOptions
            {
                Id = Context.User.Id,
                Group = DiscordSettings.GenericSettingDmGroup
            };

            var result = await _queueService.RemoveFromQueue(options, subreddit);
            await ReplyAsync(result);
        }

        [Command("q-clear")]
        [Alias("q-clear")]
        public async Task ClearQueue()
        {
            var options = new QueueOptions
            {
                Id = Context.User.Id,
                Group = DiscordSettings.GenericSettingDmGroup
            };

            var result = _queueService.ClearQueue(options);
            await ReplyAsync(result);
        }

        #endregion
    }
}
