using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueClient : IQueueClient
    {
        #region Constructor

        public QueueClient(IRedditApiService redditApiService, IEventPublisher eventPublisher)
        {
            _redditApiService = redditApiService;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Fields

        public ulong ClientId { get; set; }
        public ICollection<QueueItem> QueueItems { get; set; }

        public bool HasQueueItems { get; set; }

        private readonly IRedditApiService _redditApiService;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Methods

        public async Task Start()
        {
            if (!QueueItems.Any())
                return;

            var tasks = new List<Task>();
            while (QueueItems.Any())
            {
                var queueItems = QueueItems.Where(x => !x.IsGettingPost && x.AmountOfPosts > 0).ToList();
                foreach (var queueItem in queueItems)
                {
                    queueItem.IsGettingPost = true;
                    await Task.Run(() =>
                    {
                        tasks.Add(ProcessQueueItem(queueItem)
                            .ContinueWith((postDtoTask) =>
                            {
                                var postDto = postDtoTask.Result;
                                if (postDto == null)
                                    return;

                                queueItem.AmountOfPosts--;
                                queueItem.LastUsedPostName = postDto.Id;
                                queueItem.IsGettingPost = false;

                                if (queueItem.AmountOfPosts == 0)
                                    QueueItems.Remove(queueItem);
                            }));
                    });
                }

                await Task.Delay(1000);
            }

            await Task.WhenAll(tasks);
            HasQueueItems = false;
        }

        #region Process

        protected virtual async Task<PostDto> ProcessQueueItem(QueueItem queueItem)
        {
            var postDto = await _redditApiService.GetOldPost(queueItem.SubredditDto.Name, queueItem.LastUsedPostName,
                queueItem.Sort, queueItem.PostType, queueItem.Id);

            if (postDto == null)
                return null;

            var postMessage = new QueueItemPostReceived
            {
                PostDto = postDto,
                QueueClient = this,
                QueueItem = queueItem
            };

            await _eventPublisher.Publish(postMessage);
            return postDto;
        }

        #endregion

        #endregion
    }
}
