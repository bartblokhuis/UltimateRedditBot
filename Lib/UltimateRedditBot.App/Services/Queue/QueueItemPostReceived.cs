using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueItemPostReceived
    {
        public IQueueClient QueueClient { get; set; }
        
        public PostDto PostDto { get; set; }

        public QueueItem QueueItem { get; set; }
    }
}
