
using System.Collections.Generic;
using System.Linq;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueManager : IQueueManager
    {
        #region Fields

        private List<IQueueClient> _queueClients = new List<IQueueClient>();

        #endregion

        #region Constructor

        public QueueManager()
        {
        }

        #endregion

        public void AddQueueClient(IQueueClient queueClient)
        {
            _queueClients.Add(queueClient);
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
