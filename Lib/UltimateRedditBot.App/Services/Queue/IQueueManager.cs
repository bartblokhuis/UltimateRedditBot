using System.Collections.Generic;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public interface IQueueManager
    {
        void AddQueueClient(IQueueClient queueClient);

        void UpdateQueueClient(IQueueClient queueClient);

        IEnumerable<IQueueClient> GetQueueClients();
    }
}
