using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Domain.Queue;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Services.Queue
{
    public class DiscordQueueService : QueueService
    {
        #region Fields

        private readonly IQueueManager _queueManager;
        private readonly IRedditApiService _redditApiService;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public DiscordQueueService(IGenericSettingService genericSettingService, ISubredditService subredditService, IQueueManager queueManager, IRedditApiService redditApiService, IEventPublisher eventPublisher)
            : base(genericSettingService, subredditService, queueManager)
        {
            _queueManager = queueManager;
            _redditApiService = redditApiService;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public override async Task<string> AddToQueue<T>(T options, string subredditName, int amountOfTimes)
        {
            if (!(options is AddToQueueDiscordOptions discordClientOptions))
                throw new Exception("Discord queue options didn't parse correctly");

            var queueClient = FindQueueClient(options.Group, options.ClientId);
            if (queueClient == null)
            {
                var queueItem = await PrepareQueueItem(subredditName, amountOfTimes);
                if (queueItem.SubredditDto == null)
                    return "Subreddit could not be found";


                queueClient = CreateDiscordQueueClient(discordClientOptions);
                queueClient.QueueItems = queueClient.QueueItems.Append(queueItem);
                _queueManager.AddQueueClient(queueClient);
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

            var newQeueItem = await PrepareQueueItem(subredditName, amountOfTimes);
            if (newQeueItem.SubredditDto == null)
                return "Subreddit could not be found";

            queueClient.QueueItems = queueClient.QueueItems.Append(newQeueItem);
            _queueManager.UpdateQueueClient(queueClient);

            return "";
        }

        #endregion

        private IQueueClient FindQueueClient(string group, ulong clientId)
        {
            var queueClients = _queueManager.GetQueueClients().OfType<IDiscordQueueClient>().ToList();
            return !queueClients.Any() ? null : queueClients.FirstOrDefault(x => x.Group == @group && x.ClientId == clientId);
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
    }
}
