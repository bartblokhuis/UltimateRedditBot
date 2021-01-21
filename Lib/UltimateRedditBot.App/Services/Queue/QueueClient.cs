using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Domain.Queue
{
    public class QueueClient : IQueueClient
    {
        #region Fields

        public ulong ClientId { get; set; }
        public IEnumerable<QueueItem> QueueItems { get; set; }

        public bool HasQueueItems { get; set; }

        private readonly IRedditApiService _redditApiService;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public QueueClient(IRedditApiService redditApiService, IEventPublisher eventPublisher)
        {
            _redditApiService = redditApiService;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public async Task Start()
        {
            if (QueueItems.Any())
            {
                await Task.Run(async () =>
                {
                    var queueItems = QueueItems.DistinctBy(x => x.SubredditDto.Id).ToList();
                    await Process(queueItems);
                });
            }

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
                    continue; //Item has been removed.

                realQueueItem.AmountOfPosts--;
            }

            QueueItems = QueueItems.Where(x => x.AmountOfPosts != 0);
        }

        protected virtual async Task ProcessQueue(IEnumerable<QueueItem> queueItems)
        {
            //var postDtoTasks = GetPostDtoTasks(queueItems);

            var tasks = queueItems.Select(queueItem => GetPostDtoTasks(queueItem).ContinueWith(async post =>
            {
                queueItem.LastUsedPostName = post.Result.Id;
                var postMessage = new QueueItemPostReceived
                {
                    PostDto = post.Result,
                    QueueClient = this,
                    QueueItem = queueItem
                };

                await _eventPublisher.Publish(postMessage);
            }));

            await Task.WhenAll(tasks);
            return;
            /*var posts = await Task.WhenAll(postDtoTasks.ToArray());

            foreach (var post in posts)
            {
                var queueItem = queueItems.FirstOrDefault(x => x.SubredditDto.Id == post.SubRedditId);
                if (queueItem == null)
                    continue;

                queueItem.LastUsedPostName = post.Id;
                var postMessage = new QueueItemPostReceived
                {
                    PostDto = post,
                    QueueClient = this,
                    QueueItem = queueItem
                };

                await _eventPublisher.Publish(postMessage);*/
        }

        protected virtual IEnumerable<Task<PostDto>> GetPostDtoTasks(IEnumerable<QueueItem> queueItems)
        {
            return queueItems.Select(queueItem => _redditApiService.GetOldPost(queueItem.SubredditDto.Name, queueItem.LastUsedPostName, queueItem.Sort, queueItem.PostType, queueItem.Id)).ToList();
        }

        protected virtual Task<PostDto> GetPostDtoTasks(QueueItem queueItem)
        {
            return _redditApiService.GetOldPost(queueItem.SubredditDto.Name, queueItem.LastUsedPostName, queueItem.Sort, queueItem.PostType, queueItem.Id);
        }

        #endregion



        #endregion
    }

    internal static class MicrosoftExtensions
    {
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
