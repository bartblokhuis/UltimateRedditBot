using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Services.Queue
{
    public class DiscordQueueService : QueueService
    {
        #region Fields

        private readonly IQueueManager _queueManager;
        private readonly IRedditApiService _redditApiService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPostHistoryService _postHistoryService;
        private readonly ISubredditService _subredditService;

        #endregion

        #region Constructor

        public DiscordQueueService(IGenericSettingService genericSettingService, ISubredditService subredditService,
            IQueueManager queueManager, IRedditApiService redditApiService, IEventPublisher eventPublisher,
            IBaseRepository<PostHistory, int, UltimateDiscordDbContext> postHistoryRepo,
            IPostHistoryService postHistoryService)
            : base(genericSettingService, subredditService, queueManager)
        {
            _subredditService = subredditService;
            _queueManager = queueManager;
            _redditApiService = redditApiService;
            _eventPublisher = eventPublisher;
            _postHistoryService = postHistoryService;
        }

        #endregion

        #region Methods

        #region Add to queue

        public override async Task<string> AddToQueue<T>(T addOptions, SubredditDto subreddit, int amountOfTimes)
        {
            if (!(addOptions is AddToQueueDiscordOptions options))
                throw new ApplicationException();

            var isGuild = options.Group.Equals(DiscordSettings.GenericSettingGuildGroup);
            var id = Convert.ToUInt64(options.ClientId);

            var postHistory = _postHistoryService.GetPostHistoryName(isGuild, id, subreddit.Id);

            var queueClient = FindQueueClient(options.Group, options.ClientId, options.ChannelId);
            if (queueClient == null)
            {
                var queueItem = await PrepareQueueItem(subreddit, postHistory, amountOfTimes);
                if (queueItem.SubredditDto == null)
                    return "Subreddit could not be found";


                queueClient = CreateDiscordQueueClient(options);
                queueClient.QueueItems.Add(queueItem);
                await _queueManager.AddQueueClient(queueClient);
                return "";
            }

            //Check if there is an existing queue item with the same subreddit
            var existingQueueItem = queueClient.QueueItems.FirstOrDefault(clientItem =>
                clientItem.SubredditDto.Name.Equals(subreddit.Name, StringComparison.OrdinalIgnoreCase));

            if (existingQueueItem != null)
            {
                existingQueueItem.AmountOfPosts += amountOfTimes;
                _queueManager.UpdateQueueClient(queueClient, client => (client as DiscordQueueClient)?.ChannelId == options.ChannelId && client.ClientId == queueClient.ClientId);
                return string.Empty;
            }

            var newQeueItem = await PrepareQueueItem(subreddit, postHistory, amountOfTimes);
            if (newQeueItem.SubredditDto == null)
                return "Subreddit could not be found";

            queueClient.QueueItems.Add(newQeueItem);
            _queueManager.UpdateQueueClient(queueClient, client => (client as DiscordQueueClient)?.ChannelId == options.ChannelId && client.ClientId == queueClient.ClientId);

            return "";
        }

        #endregion

        #region Get Queue Client

        public override IQueueClient GetQueueClient<T>(T getOptions)
        {
            if (!(getOptions is QueueOptions options))
                throw new ApplicationException();

            return FindQueueClient(options.Group, options.Id, options.ChannelId);
        }

        #endregion

        #region Clear queue

        public override string ClearQueue<T>(T identifier)
        {
            if (!(identifier is QueueOptions options))
                throw new ApplicationException();

            var queClient = FindQueueClient(options.Group, options.Id, options.ChannelId);
            if (queClient == null)
                return "No items in the queue";

            queClient.QueueItems = new List<QueueItem>();
            return "Cleared the queue";
        }

        #endregion

        #region Remove from queue

        public override async Task<string> RemoveFromQueue<T>(T identifier, string subredditName)
        {
            if (!(identifier is QueueOptions options))
                throw new ApplicationException();

            var queClient = FindQueueClient(options.Group, options.Id, options.ChannelId);
            if (queClient == null)
                return "No items in the queue";

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
                return "Subreddit doesn't exist";

            var queueItem = queClient.QueueItems.FirstOrDefault(x => x.SubredditDto.Id == subreddit.Id);
            if (queueItem == null)
                return "Subreddit not in queue";

            queClient.QueueItems.Remove(queueItem);
            return "Removed subreddit from queue";
        }

        #endregion

        #endregion

        #region Utils

        private IQueueClient FindQueueClient(string group, ulong clientId, ulong? channelId = null)
        {
            var queueClients = _queueManager.GetQueueClients().OfType<IDiscordQueueClient>().ToList();

            if (!queueClients.Any())
                return null;

            return (group == DiscordSettings.GenericSettingGuildGroup)
                ? queueClients.FirstOrDefault(x =>
                    x.Group == group && x.ClientId == clientId && x.ChannelId == channelId)

                : queueClients.FirstOrDefault(x => x.Group == group && x.ClientId == clientId);
        }

        private DiscordQueueClient CreateDiscordQueueClient(IAddToQueueDiscordOptions options)
        {
            return new(_redditApiService, _eventPublisher)
            {
                Group = options.Group,
                ClientId = options.ClientId,
                ChannelId = options.ChannelId,
                QueueItems = new List<QueueItem>()
            };
        }

        #endregion




    }
}
