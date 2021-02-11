using System.Collections.Generic;

namespace UltimateRedditBot.Domain.Queue
{
    public interface IQueueClient
    {
        ulong ClientId { get; set; }

        IEnumerable<QueueItem> QueueItems { get; set; }

        bool HasQueueItems { get; }
    }
}