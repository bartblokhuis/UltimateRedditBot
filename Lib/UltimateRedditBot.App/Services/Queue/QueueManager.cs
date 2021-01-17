
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueManager : IQueueManager
    {
        #region Fields

        private readonly List<IQueueClient> _queueClients = new List<IQueueClient>();

        #endregion

        #region Constructor

        public QueueManager()
        {
        }

        public void AddToQueue(QueueItem queueItem, ulong queClientId)
        {
            var queueClient = _queueClients.FirstOrDefault(client => client.QueueClientId == queClientId);

            if (queueClient == null)
            {
                //New client
            }
            else
            {
                queueClient.QueueItems = queueClient.QueueItems.Append(queueItem);
            }
        }

        #endregion
    }
}
