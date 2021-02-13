using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueManager : IQueueManager
    {
        #region Fields

        private readonly List<IQueueClient> _queueClients = new();

        #endregion

        public async Task AddQueueClient(IQueueClient queueClient)
        {
            _queueClients.Add(queueClient);
            if (!(queueClient is QueueClient client))
                throw new ApplicationException();

            await client.Start();
        }

        public void UpdateQueueClient(IQueueClient queueClient)
        {
            var oldClient = _queueClients.FirstOrDefault(client => client.ClientId == queueClient.ClientId);
            if (oldClient == null)
            {
                AddQueueClient(queueClient);
                return;
            }

            oldClient.QueueItems = queueClient.QueueItems;

            if (!queueClient.HasQueueItems)
            {
                queueClient.HasQueueItems = true;
                queueClient.Start();

            }

        }

        public IEnumerable<IQueueClient> GetQueueClients()
        {
            return _queueClients;
        }

        #region Constructor

        #endregion
    }
}
