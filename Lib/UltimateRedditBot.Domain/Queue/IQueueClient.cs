using System.Collections.Generic;
using System.Threading.Tasks;

namespace UltimateRedditBot.Domain.Queue
{
    public interface IQueueClient
    {
        ulong ClientId { get; set; }

        ICollection<QueueItem> QueueItems { get; set; }

        bool HasQueueItems { get; set; }

        Task Start();
    }
}
