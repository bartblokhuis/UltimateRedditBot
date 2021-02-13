using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
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

        #region Add TO queue

        public override async Task<string> AddToQueue<T>(T options, string subredditName, int amountOfTimes)
        {
            if (!(options is AddToQueueDiscordOptions discordClientOptions))
                throw new Exception("Discord queue options didn't parse correctly");

            var isGuild = (options as AddToQueueDiscordOptions).Group.Equals(DiscordSettings.GenericSettingGuildGroup);
            var id = Convert.ToUInt64((options as AddToQueueDiscordOptions).ClientId);

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            var postHistory = _postHistoryService.GetPostHistoryName(isGuild, id, subreddit.Id);

            var queueClient = FindQueueClient(options.Group, options.ClientId);
            if (queueClient == null)
            {
                var queueItem = await PrepareQueueItem(subredditName, postHistory, amountOfTimes);
                if (queueItem.SubredditDto == null)
                    return "Subreddit could not be found";


                queueClient = CreateDiscordQueueClient(discordClientOptions);
                queueClient.QueueItems.Add(queueItem);
                await _queueManager.AddQueueClient(queueClient);
                return "";
            }

            //Check if there is an existing queue item with the same subreddit
            var existingQueueItem = queueClient.QueueItems.FirstOrDefault(clientItem =>
                clientItem.SubredditDto.Name.Equals(subredditName, StringComparison.OrdinalIgnoreCase));

            if (existingQueueItem != null)
            {
                existingQueueItem.AmountOfPosts += amountOfTimes;
                _queueManager.UpdateQueueClient(queueClient);
                return string.Empty;
            }

            var newQeueItem = await PrepareQueueItem(subredditName, postHistory, amountOfTimes);
            if (newQeueItem.SubredditDto == null)
                return "Subreddit could not be found";

            queueClient.QueueItems.Add(newQeueItem);
            _queueManager.UpdateQueueClient(queueClient);

            return "";
        }

        #endregion

        #region Get Queue Client

        public override IQueueClient GetQueueClient<T>(T getOptions)
        {
            if (!(getOptions is GetQueueDto options))
                throw new ApplicationException();

            return FindQueueClient(options.Group, options.Id, options.ChannelId);
        }

        #endregion

        #region Clear queue

        public override string ClearQueue<T>(T identifier)
        {
            if (!(identifier is GetQueueDto options))
                throw new ApplicationException();
        }

        #endregion

        #endregion

        #region Utils

        private IQueueClient FindQueueClient(string group, ulong clientId, ulong? channelId = null)
        {
            var queueClients = _queueManager.GetQueueClients().OfType<IDiscordQueueClient>().ToList();
            return !queueClients.Any()
                ? null
                : queueClients.FirstOrDefault(x => x.Group == group && x.ClientId == clientId
                                                                    && (channelId.HasValue)? x.ChannelId == channelId: true );
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
