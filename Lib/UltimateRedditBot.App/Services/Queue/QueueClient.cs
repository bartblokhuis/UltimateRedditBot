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
        public IEnumerable<QueueItem> QueueItems { get; set; }

        public bool HasQueueItems { get; set; }

        private readonly IRedditApiService _redditApiService;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Methods

        public async Task Start()
        {
            if (QueueItems.Any())
                await Task.Run(async () =>
                {
                    var queueItems = QueueItems.DistinctBy(x => x.SubredditDto.Id).ToList();
                    await Process(queueItems);
                });

            //Wait one second before starting again
            await Task.Delay(1000);
            Start();
        }

        #region Process

        protected virtual async Task Process(ICollection<QueueItem> queueItems)
        {
            if (!queueItems.Any())
                return;

            await ProcessQueue(queueItems);
            foreach (var queueItem in queueItems)
            {
                var realQueueItem = QueueItems.FirstOrDefault(x => x.Id == queueItem.Id);
                if (realQueueItem is null)
                    continue;

                realQueueItem.AmountOfPosts--;
            }

            QueueItems = QueueItems.Where(x => x.AmountOfPosts != 0);
        }

        protected virtual Task ProcessQueue(IEnumerable<QueueItem> queueItems)
        {
            var postDtos = queueItems.Select(GetPostDtoTasks);
            return Task.WhenAll(postDtos);
        }

        //TODO Better method naming and documentation
        protected virtual async Task<PostDto> GetPostDtoTasks(QueueItem queueItem)
        {
            var postDto = await _redditApiService.GetOldPost(queueItem.SubredditDto.Name, queueItem.LastUsedPostName,
                queueItem.Sort, queueItem.PostType, queueItem.Id);
            if (postDto == null)
                return null;

            queueItem.LastUsedPostName = postDto.Id;
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

    internal static class MicrosoftExtensions
    {
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }
    }
}
