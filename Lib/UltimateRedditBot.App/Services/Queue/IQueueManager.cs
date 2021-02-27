using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public interface IQueueManager
    {
        Task AddQueueClient(IQueueClient queueClient);

        void UpdateQueueClient(IQueueClient queueClient, Func<IQueueClient, bool> predicate);

        IEnumerable<IQueueClient> GetQueueClients();
    }
}
