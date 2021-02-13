using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Queue;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Services.Queue
{
    public abstract class QueueService : IQueueService
    {
        #region Fields

        private readonly IGenericSettingService _genericSettingService;
        private readonly ISubredditService _subredditService;
        private readonly IQueueManager _queueManager;

        #endregion

        #region Constructor

        public QueueService(IGenericSettingService genericSettingService, ISubredditService subredditService,
            IQueueManager queueManager)
        {
            _genericSettingService = genericSettingService;
            _subredditService = subredditService;
            _queueManager = queueManager;
        }

        #endregion

        public virtual async Task<string> AddToQueue<T>(T options, string subredditName, int amountOfTimes)
            where T : IAddToQueueOptions
        {
            var max = await _genericSettingService.GetSettingValueByKeyGroupAndKey<int>(options.Group,
                GenericSettingKeyConstants.BulkSettingKey, options.ClientId.ToString());

            if (max > amountOfTimes)
                amountOfTimes = max;

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
                return "Subreddit doesn't exist";

            var queueClient = FindQueueClient(1);

            return string.Empty;
        }

        public virtual IQueueClient GetQueueClient<T>(T identifier)
        {
            throw new NotImplementedException();
        }

        public virtual string ClearQueue<T>(T identifier)
        {
            throw new NotImplementedException();
        }

        protected virtual async Task<QueueItem> PrepareQueueItem(string subredditName, string lastPostName,
            int amountOfPosts = 1)
        {
            lastPostName ??= "";
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            return new QueueItem
            {
                Id = Guid.NewGuid(),
                Sort = Sort.Hot,
                PostType = PostType.Gif,
                SubredditDto = subreddit,
                AmountOfPosts = amountOfPosts,
                LastUsedPostName = lastPostName
            };
        }

        protected virtual IQueueClient FindQueueClient(ulong id)
        {
            return null;
        }
    }
}
