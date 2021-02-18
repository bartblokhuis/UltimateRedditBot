using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Domain.Dtos.Reddit;
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

        public virtual async Task<string> AddToQueue<T>(T options, SubredditDto subreddit, int amountOfTimes)
            where T : IAddToQueueOptions
        {
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

        public virtual Task<string> RemoveFromQueue<T>(T identifier, string subredditName)
        {
            throw new NotImplementedException();
        }

        protected virtual async Task<QueueItem> PrepareQueueItem(SubredditDto subreddit, string lastPostName,
            int amountOfPosts = 1)
        {
            lastPostName ??= "";
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
