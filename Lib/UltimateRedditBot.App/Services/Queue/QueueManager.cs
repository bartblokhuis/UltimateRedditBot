
using System.Collections.Generic;
using System.Linq;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueManager : IQueueManager
    {
        #region Fields

        private readonly List<IQueueClient> _queueClients = new();

        #endregion

        #region Constructor

        #endregion

        public void AddQueueClient(IQueueClient queueClient)
        {
            _queueClients.Add(queueClient);
            var client = queueClient as QueueClient;

            client?.Start();
        }

        public void UpdateQueueClient(IQueueClient queueClient)
        {
            var oldClient = _queueClients.FirstOrDefault(client => client.ClientId == queueClient.ClientId);
        }

        public IEnumerable<IQueueClient> GetQueueClients()
        {
            return _queueClients;
        }
    }
}
