using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Discord.Modules.Helpers;
using UltimateRedditBot.Discord.App.Services.Queue;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessagesQueueModule : UltimateDirectMessageModule
    {
        #region Fields

        private readonly IQueueService _queueService;

        #endregion

        #region Constructor

        public DirectMessagesQueueModule(IQueueService queueService)
        {
            _queueService = queueService;
        }

        #endregion

        #region Methods

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subreddit)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingDmGroup,
                ClientId = Context.User.Id
            };
            var result = await _queueService.AddToQueue(options, subreddit, 1);
            await ReplyAsync(result);
        }

        [Command("r")]
        [Alias("r")]
        public async Task AddToQueue(string subreddit, int amountOfTimes)
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingDmGroup,
                ClientId = Context.User.Id
            };
            var result = await _queueService.AddToQueue(options, subreddit, amountOfTimes);
            await ReplyAsync(result);
        }

        #region Get Queue

        [Command("queue")]
        [Alias("q")]
        public async Task GetQueue()
        {
            var options = new GetQueueDto
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
