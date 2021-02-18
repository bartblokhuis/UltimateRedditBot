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
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildQueueModule : UltimateGuildModule
    {
        #region Fields

        private readonly IBannedSubredditService _bannedSubredditService;
        private readonly IQueueService _queueService;
        private readonly ISubredditService _subredditService;

        #endregion

        #region Constructor

        public GuildQueueModule(IQueueService queueService, IBannedSubredditService bannedSubredditService, ISubredditService subredditService)
        {
            _queueService = queueService;
            _bannedSubredditService = bannedSubredditService;
            _subredditService = subredditService;
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

        [Command("r-random")]
        [Alias("r-r")]

        public async Task AddRandomToQueue()
        {
            var options = new AddToQueueDiscordOptions
            {
                Group = DiscordSettings.GenericSettingGuildGroup,
                ClientId = Context.Guild.Id,
                ChannelId = Context.Channel.Id
            };

            var bannedSubreddits = await _bannedSubredditService.GetBannedSubredditIds(Context.Guild.Id);
            var subredditName = await _subredditService.GetRandomSubredditName(((ITextChannel) Context.Channel).IsNsfw, bannedSubreddits);
            await AddToQueue(options, subredditName, 1);
        }

        private async Task AddToQueue(AddToQueueDiscordOptions options, string subredditName, int amountOfTimes)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            if (_bannedSubredditService.IsSubredditBanned(Context.Guild.Id, subreddit))
            {
                await ReplyAsync("This subreddit is banned");
                return;
            }

            if (subreddit.IsNsfw && !((ITextChannel) Context.Channel).IsNsfw)
            {
                await ReplyAsync("Can't post nsfw subreddits in a non nsfw channel");
                return;
            }

            await _queueService.AddToQueue(options, subreddit, amountOfTimes);
        }

        #endregion

        #region Get Queue

        [Command("queue")]
        [Alias("q")]
        public async Task GetQueue()
        {
            var options = new QueueOptions
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

        #region Remove subreddit from queue

        [Command("q-remove")]
        [Alias("q-r")]
        public async Task RemoveFromQueue(string subreddit)
        {
            var options = new QueueOptions
            {
                Id = Context.Guild.Id,
                ChannelId = Context.Channel.Id,
                Group = DiscordSettings.GenericSettingGuildGroup
            };

            var result = await _queueService.RemoveFromQueue(options, subreddit);
            await ReplyAsync(result);
        }

        #endregion

        #region Clear Queue

        [Command("q-clear")]
        [Alias("q-c")]
        public async Task ClearQueue()
        {
            var options = new QueueOptions
            {
                Id = Context.Guild.Id,
                ChannelId = Context.Channel.Id,
                Group = DiscordSettings.GenericSettingGuildGroup
            };

            var result = _queueService.ClearQueue(options);
            await ReplyAsync(result);
        }


        #endregion

        #endregion
    }
}
